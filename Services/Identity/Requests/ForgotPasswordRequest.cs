using FluentValidation;
using Identity.Extensions;

namespace Identity.Requests;


public sealed record ForgotPasswordRequest(string Email, string ResetUrl)
{
    internal sealed class Validator : AbstractValidator<ForgotPasswordRequest>
    {
        public Validator() => RuleFor(x => x.Email).IsValidEmail();
    }
}