using Common.Other;
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
            await TrySeedAsync().ConfigureAwait(false);
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
            if (await roleManager.FindByNameAsync(role).ConfigureAwait(false) is null)
                await roleManager.CreateAsync(new Role { Name = role }).ConfigureAwait(false);

        if (await userManager.FindByEmailAsync(_options.Email).ConfigureAwait(false) is not null) return;

        var admin = new User
        {
            Email = _options.Email,
            UserName = _options.UserName
        };

        var result = await userManager.CreateAsync(admin, _options.Password).ConfigureAwait(false);

        if (!result.Succeeded) return;

        var token = await userManager.GenerateEmailConfirmationTokenAsync(admin).ConfigureAwait(false);
        await userManager.ConfirmEmailAsync(admin, token).ConfigureAwait(false);
        await userManager.AddToRoleAsync(admin, Roles.Admin).ConfigureAwait(false);
    }
}