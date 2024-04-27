using Common;
using Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Persistence;

internal sealed class ApplicationContextInitializer(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IOptions<AdminOptions> options,
    ILogger<ApplicationContextInitializer> logger
)
{
    private readonly AdminOptions _options = options.Value;

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
        foreach (var role in Roles.All)
            if (await roleManager.FindByNameAsync(role) is null)
                await roleManager.CreateAsync(new Role { Name = role });

        if (await userManager.FindByEmailAsync(_options.Email) is not null) return;

        var admin = new User
        {
            Email = _options.Email,
            UserName = _options.UserName
        };

        var result = await userManager.CreateAsync(admin, _options.Password);

        if (!result.Succeeded) return;

        var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
        await userManager.ConfirmEmailAsync(admin, token);
        await userManager.AddToRoleAsync(admin, Roles.Admin);
    }
}