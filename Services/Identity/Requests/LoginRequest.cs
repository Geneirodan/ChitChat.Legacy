using FluentValidation;

namespace Identity.Requests;


internal sealed record LoginRequest(
    string Username,
    string Password,
    string? TwoFactorCode = null,
    string? TwoFactorRecoveryCode = null)
{
    internal class Validator : AbstractValidator<LoginRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}