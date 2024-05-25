using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;


internal sealed record ChangePasswordRequest(string NewPassword, string OldPassword)
{
    internal class Validator : AbstractValidator<ChangePasswordRequest>
    {
        public Validator(IOptions<IdentityOptions> options)
        {
            RuleFor(x => x.NewPassword).IsValidPassword(options.Value.Password);
            RuleFor(x => x.OldPassword).NotEmpty();
        }
    }
    
}
