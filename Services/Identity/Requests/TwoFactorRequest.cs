using FluentValidation;

namespace Identity.Requests;

internal sealed record TwoFactorRequest(string TwoFactorCode)
{
    internal sealed class Validator : AbstractValidator<TwoFactorRequest>
    {
        public Validator() => RuleFor(x => x.TwoFactorCode).NotEmpty();
    }
}