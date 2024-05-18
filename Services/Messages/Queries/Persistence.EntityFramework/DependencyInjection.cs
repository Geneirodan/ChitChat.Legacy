using Messages.Queries.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Messages.Queries.Persistence.EntityFramework;

public static class DependencyInjection
{
    public static IServiceCollection AddEntityFrameworkCore(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<ApplicationContext>(optionsAction);
        return services
            .AddScoped<IMessageQueryHandler, MessageQueryHandler>()
            .AddScoped<IMessageRepository, MessageRepository>();
    }
}