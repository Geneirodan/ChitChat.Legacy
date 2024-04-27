namespace Identity.Requests;

internal sealed record TwoFactorRequest(
    bool? Enable,
    string? TwoFactorCode,
    bool ResetSharedKey = false,
    bool ResetRecoveryCodes = false,
    bool ForgetMachine = false);