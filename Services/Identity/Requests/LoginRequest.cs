namespace Identity.Requests;


internal sealed record LoginRequest(
    string Username,
    string Password,
    bool IsPersistent = true,
    string? TwoFactorCode = null,
    string? TwoFactorRecoveryCode = null);