using Common.Http;
using MassTransit;
using Messages.Commands.Application;
using Messages.Commands.Infrastructure.Marten;
using Messages.Commands.Infrastructure.MassTransit;
using Messages.Commands.Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("Database");
services.AddHealthChecks();

services
    .AddHttpUser()
    .AddApplicationServices()
    .Configure<RabbitMqTransportOptions>(configuration.GetSection("RabbitMQ"))
    .AddMassTransit()
    .AddInfrastructure(connectionString!)
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

app.MapMessages("api/v1/messages");

await app.RunAsync().ConfigureAwait(false);

public partial class Program;