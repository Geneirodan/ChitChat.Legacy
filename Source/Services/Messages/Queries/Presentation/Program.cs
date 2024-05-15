using Common.Http;
using GraphQL.AspNet.Configuration;
using Messages.Queries.Persistence.Elastic;
using Messages.Queries.Persistence.EntityFramework;
using Messages.Queries.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var rabbitMqSection = configuration.GetSection("RabbitMQ");
var jwtSection = configuration.GetSection("Jwt");

if (Environment.GetEnvironmentVariable("USE_EF") == "true")
    builder.Services.AddEntityFrameworkCore(o => o.UseNpgsql(configuration.GetConnectionString("Database")!));
else
    builder.Services.AddElastic(configuration.GetConnectionString("Elastic")!);

builder.Services
    .AddMassTransit(rabbitMqSection)
    .AddApplicationServices()
    .AddHttpUser()
    .AddProblemDetails()
    .AddSwagger()
    .AddAuthenticationAndAuthorization(jwtSection)
    .AddGraphQL();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
    app.UseGraphQLGraphiQL();
}

app.UseExceptionHandler()
    .UseStatusCodePages()
    .UseAuthentication()
    .UseAuthorization();

app.MapMessages("api/v1/messages");

app.UseGraphQL();
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<IndexInitializer>().InitializeAsync();
await app.RunAsync().ConfigureAwait(false);

public partial class Program;