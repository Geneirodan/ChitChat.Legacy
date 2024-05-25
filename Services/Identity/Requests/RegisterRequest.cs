using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;

internal sealed record RegisterRequest(string Username, string Email, string Password, string ConfirmUrl)
{
    internal class Validator : AbstractValidator<RegisterRequest>
    {
        public Validator(IOptions<IdentityOptions> options)
        {
            RuleFor(x => x.Email).IsValidEmail();
            RuleFor(x => x.Username).IsValidUsername();
            RuleFor(x => x.Password).IsValidPassword(options.Value.Password);
            if (options.Value.SignIn.RequireConfirmedEmail)
                RuleFor(x => x.ConfirmUrl).NotEmpty();
        }
    }
}