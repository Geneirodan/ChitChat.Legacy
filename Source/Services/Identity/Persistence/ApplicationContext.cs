using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence;

internal sealed class ApplicationContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
}