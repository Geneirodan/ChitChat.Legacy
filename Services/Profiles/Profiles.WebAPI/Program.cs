using Common.Http;
using MassTransit;
using Profiles.Application;
using Profiles.Infrastructure.EntityFramework;
using Profiles.Infrastructure.MassTransit;
using Profiles.WebAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("Database");

services
    .AddRepositories(connectionString!)
    .AddHttpUser()
    .AddApplicationServices()
    .Configure<RabbitMqTransportOptions>(configuration.GetSection("RabbitMQ"))
    .AddMassTransitServices()
    .AddProblemDetails()
    .AddSwagger()
    .AddAuthenticationAndAuthorization(configuration.GetSection("Jwt"));

services.AddHealthChecks();

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

namespace Profiles.WebAPI
{
    public partial class Program;
}