namespace Identity.Responses;

internal sealed record TwoFactorResponse(
    string SharedKey,
    int RecoveryCodesLeft,
    bool IsTwoFactorEnabled,
    bool IsMachineRemembered,
    string[]? RecoveryCodes);