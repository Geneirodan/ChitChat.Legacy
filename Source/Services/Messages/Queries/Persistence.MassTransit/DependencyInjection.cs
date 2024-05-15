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
            const string prefix = "MessagesQueries";
            configurator.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(prefix));
            configurator.AddConsumers(Assembly.GetExecutingAssembly());
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseInMemoryOutbox(context);
                cfg.ConfigureEndpoints(context);
            });
        });
        return services.Configure<RabbitMqTransportOptions>(configuration);
    }
}