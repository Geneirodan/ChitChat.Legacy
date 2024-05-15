using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(configurator =>
        {
            const string prefix = "MessagesCommands";
            configurator.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(prefix));
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseInMemoryOutbox(context);
                cfg.ConfigureEndpoints(context);
            });
        });
        services.Configure<RabbitMqTransportOptions>(configuration);
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}