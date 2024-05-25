using FluentValidation;
using Identity.Extensions;

namespace Identity.Requests;

internal sealed record ResendEmailRequest(string Email, string ConfirmUrl)
{
    internal sealed class Validator : AbstractValidator<ResendEmailRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Email).IsValidEmail();
            RuleFor(x => x.ConfirmUrl).NotEmpty();
        }
    }
}