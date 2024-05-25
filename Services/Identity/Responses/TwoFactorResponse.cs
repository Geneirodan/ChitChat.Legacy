namespace Identity.Responses;

internal sealed record TwoFactorResponse(string SharedKey, IEnumerable<string>? RecoveryCodes);