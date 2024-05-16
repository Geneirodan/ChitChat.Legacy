using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Profile.Application.Interfaces;
using Profile.Infrastructure.Marten.Projections;
using Profile.Infrastructure.Marten.Repositories;
using Weasel.Core;

namespace Profile.Infrastructure.Marten;

public static class DependencyInjection
{
    public static IServiceCollection AddMarten(this IServiceCollection services, string connectionString)
    {
        services
            .AddMarten(options =>
            {
                options.Connection(connectionString);
                options.UseSystemTextJsonForSerialization(EnumStorage.AsString,Casing.CamelCase);
                options.Projections.Add<ProfileProjection>(ProjectionLifecycle.Inline);
            })
            .UseLightweightSessions()
            .OptimizeArtifactWorkflow();

        return services.AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IProfileRepository, ProfileRepository>();

}