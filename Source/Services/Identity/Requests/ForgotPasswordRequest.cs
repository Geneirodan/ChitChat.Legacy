using FluentValidation;
using Identity.Extensions;

namespace Identity.Requests;


public sealed record ForgotPasswordRequest(string Email, string ResetUrl);

internal sealed class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator() => RuleFor(x => x.Email).IsValidEmail();
}