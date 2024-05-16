using Common.Http;
using Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("Database");
services.AddHealthChecks();

services
    .AddHttpUser()
    .AddApplicationServices()
    .AddMassTransit(configuration.GetSection("RabbitMQ"))
    .AddMarten(connectionString!)
    .AddProblemDetails()
    .AddSwagger()
    .AddAuthenticationAndAuthorization(configuration.GetSection("Jwt"));

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
    app.UseSwagger().UseSwaggerUI();

app.UseExceptionHandler()
    .UseStatusCodePages()
    .UseAuthentication()
    .UseAuthorization();

app.MapProfile("api/v1/profile");

await app.RunAsync().ConfigureAwait(false);

public partial class Program;