using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;


internal sealed record ResetPasswordRequest(string Email, string ResetCode, string NewPassword)
{
    internal class Validator : AbstractValidator<ResetPasswordRequest>
    {
        public Validator(IOptions<IdentityOptions> options) => 
            RuleFor(x => x.NewPassword).IsValidPassword(options.Value.Password);
    }
}