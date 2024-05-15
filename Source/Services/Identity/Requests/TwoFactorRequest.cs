using FluentValidation;

namespace Identity.Requests;

internal sealed record TwoFactorRequest(
    bool? Enable,
    string? TwoFactorCode,
    bool ResetSharedKey = false,
    bool ResetRecoveryCodes = false,
    bool ForgetMachine = false);

// ReSharper disable once UnusedType.Global
internal sealed class TwoFactorRequestValidator : AbstractValidator<TwoFactorRequest>
{
    public TwoFactorRequestValidator() =>
        When(x => x.Enable == true, () =>
        {
            RuleFor(x => x.ResetSharedKey).Empty();
            RuleFor(x => x.TwoFactorCode).NotEmpty();
        });
}