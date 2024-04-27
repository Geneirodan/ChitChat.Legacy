namespace Identity.Requests;

internal sealed record LoginRequest(string Username, string Password, bool IsPersistent, string? TwoFactorCode, string? TwoFactorRecoveryCode);