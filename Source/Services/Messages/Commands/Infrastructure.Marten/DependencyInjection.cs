using Messages.Commands.Application.Interfaces;
using Messages.Commands.Infrastructure.Marten.Repositories;
using Weasel.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services
            .AddMarten(options =>
            {
                options.Connection(connectionString);
                options.UseSystemTextJsonForSerialization(
                    enumStorage: EnumStorage.AsString,
                    casing: Casing.CamelCase
                );

                //options.Projections.Add<MessageProjection>(ProjectionLifecycle.Inline);
            })
            .UseLightweightSessions()
            .OptimizeArtifactWorkflow();

        return services.AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IMessageRepository, MessageRepository>();

}