using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;

internal sealed record ResetPasswordRequest(string Email, string ResetCode, string NewPassword);

internal class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator(IOptions<IdentityOptions> options) => 
        RuleFor(x => x.NewPassword).IsValidPassword(options.Value.Password);
}