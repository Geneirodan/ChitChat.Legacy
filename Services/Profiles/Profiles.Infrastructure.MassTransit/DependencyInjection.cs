using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Profiles.Infrastructure.MassTransit;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransitServices(this IServiceCollection services)
    {
        services.AddMassTransit(configurator =>
        {
            const string prefix = "ProfileCommands";
            configurator.SetEndpointNameFormatter(new DefaultEndpointNameFormatter(prefix));
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseInMemoryOutbox(context);
                cfg.ConfigureEndpoints(context);
            });
        });
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}