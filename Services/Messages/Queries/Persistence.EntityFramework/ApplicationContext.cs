using System.Reflection;
using Messages.Queries.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messages.Queries.Persistence.EntityFramework;

public sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Message> Messages { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}