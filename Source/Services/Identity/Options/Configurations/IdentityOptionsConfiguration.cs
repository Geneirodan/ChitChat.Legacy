using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Options.Configurations;

internal class IdentityOptionsConfiguration : IConfigureOptions<IdentityOptions>
{
    public void Configure(IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = options.User.AllowedUserNameCharacters.Replace("@", string.Empty);
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedEmail = true;
    }
}