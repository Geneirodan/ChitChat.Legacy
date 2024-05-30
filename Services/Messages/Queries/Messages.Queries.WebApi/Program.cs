using Common.Http;
using GraphQL.AspNet.Configuration;
using MassTransit;
using Messages.Queries.Application;
using Messages.Queries.Infrastructure.Elastic;
using Messages.Queries.Infrastructure.MassTransit;
using Messages.Queries.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var rabbitMqSection = configuration.GetSection("RabbitMQ");
var jwtSection = configuration.GetSection("Jwt");

builder.Services.AddElastic(configuration.GetConnectionString("Elastic")!);

builder.Services
    .Configure<RabbitMqTransportOptions>(configuration)
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
    await scope.ServiceProvider.GetRequiredService<IndexInitializer>().InitializeAsync().ConfigureAwait(false);
await app.RunAsync().ConfigureAwait(false);

namespace Messages.Queries.WebApi
{
    public partial class Program;
}