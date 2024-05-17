using Common.Http;
using MassTransit;
using Profiles.Application;
using Profiles.Infrastructure.Marten;
using Profiles.Infrastructure.MassTransit;
using Profiles.Presentation.Endpoints;

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
app.MapContacts("api/v1/contacts");

await app.RunAsync().ConfigureAwait(false);

public partial class Program;