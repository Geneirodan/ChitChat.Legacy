using FluentValidation;
using Identity.Extensions;

namespace Identity.Requests;


internal sealed record ChangeEmailRequest(string Email, string ReturnUrl)
{
    internal sealed class Validator : AbstractValidator<ChangeEmailRequest>
    {
        public Validator()
        {
            RuleFor(x => x.ReturnUrl).NotEmpty();
            RuleFor(x => x.Email).IsValidEmail();
        }
    }
}
