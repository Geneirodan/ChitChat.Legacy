using FluentValidation;
using Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Requests;

internal sealed record ChangePasswordRequest(string NewPassword, string OldPassword);

// ReSharper disable once UnusedType.Global
internal class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator(IOptions<IdentityOptions> options) => 
        RuleFor(x => x.NewPassword).IsValidPassword(options.Value.Password);
}