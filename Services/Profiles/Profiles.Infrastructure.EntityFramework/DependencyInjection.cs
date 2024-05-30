using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Interfaces;
using Profiles.Infrastructure.EntityFramework.Repositories;

namespace Profiles.Infrastructure.EntityFramework;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDbContext<ApplicationContext>(x => x.UseNpgsql(connectionString))
            .AddScoped<IContactsRepository, ContactsRepository>()
            .AddScoped<IContactsReadRepository, ContactsReadRepository>()
            .AddScoped<IProfilesRepository, ProfilesRepository>()
            .AddScoped<IProfilesReadRepository, ProfilesReadRepository>();
    }
}