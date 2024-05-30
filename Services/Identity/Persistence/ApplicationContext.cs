using Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Persistence;

internal sealed class ApplicationContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
    
    internal sealed class Initializer(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<Initializer> logger
    )
    {
        private readonly AdminOptions _adminOptions = adminOptions.Value;

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task TrySeedAsync()
        {
            foreach (var role in Common.Roles.All)
                if (await roleManager.FindByNameAsync(role) is null)
                    await roleManager.CreateAsync(new Role { Name = role });

            if (await userManager.FindByEmailAsync(_adminOptions.Email) is not null) return;

            var admin = new User
            {
                Email = _adminOptions.Email,
                UserName = _adminOptions.UserName
            };

            var result = await userManager.CreateAsync(admin, _adminOptions.Password);

            if (!result.Succeeded) return;

            var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
            await userManager.ConfirmEmailAsync(admin, token);
            await userManager.AddToRoleAsync(admin, Common.Roles.Admin);
        }
    }
}