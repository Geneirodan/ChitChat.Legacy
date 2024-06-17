using Common.Http;
using MassTransit;
using MassTransit.SignalR;
using Messages.SignalRNotifier.EventConsumers;
using Messages.SignalRNotifier.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddMassTransit(configurator =>
        {
            const string prefix = "MessagesSignalR";
            configurator.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(prefix));
            configurator.AddSignalRHub<ChatHub>();
            configurator.AddConsumersFromNamespaceContaining(typeof(MessageEventConsumer<>));
            configurator.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
        }
    )
    .Configure<RabbitMqTransportOptions>(configuration.GetSection("RabbitMQ"))
    .AddAuthenticationAndAuthorization(configuration.GetSection("Jwt"))
    .AddCors()
    .AddSignalR();

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithOrigins("https://gourav-d.github.io"));
app.MapHub<ChatHub>("api/v1/chat");

await app.RunAsync().ConfigureAwait(false);