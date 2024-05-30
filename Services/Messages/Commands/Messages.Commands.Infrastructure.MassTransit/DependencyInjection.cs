using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Messages.Commands.Infrastructure.MassTransit;

public static class DependencyInjection
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
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
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}