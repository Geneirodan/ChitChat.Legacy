using FluentValidation;
using Identity.Extensions;

namespace Identity.Requests;

internal sealed record ChangeEmailRequest(string Email, string ReturnUrl);

// ReSharper disable once UnusedType.Global
internal sealed class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
{
    public ChangeEmailRequestValidator() => RuleFor(x => x.Email).IsValidEmail();
}