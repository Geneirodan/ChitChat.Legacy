using Marten.Events.Projections;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Interfaces;
using Profiles.Infrastructure.Marten.Projections;
using Profiles.Infrastructure.Marten.Repositories;
using Weasel.Core;

namespace Profiles.Infrastructure.Marten;

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
                options.Projections.Add<ContactProjection>(ProjectionLifecycle.Inline);
            })
            .UseLightweightSessions()
            .OptimizeArtifactWorkflow();

        return services.AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IProfileRepository, ProfileRepository>()
            .AddScoped<IContactRepository, ContactRepository>();

}