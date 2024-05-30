using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.EntityFramework;

public sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; init; } = null!;
    public DbSet<Profile> Profiles { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}