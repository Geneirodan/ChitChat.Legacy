using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;


internal sealed record RegisterRequest(string Username, string Email, string Password, string ConfirmUrl);

internal class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IOptions<IdentityOptions> options)
    {
        RuleFor(x => x.Email).IsValidEmail();
        RuleFor(x => x.Username).IsValidUsername();
        RuleFor(x => x.Password).IsValidPassword(options.Value.Password);
    }
}