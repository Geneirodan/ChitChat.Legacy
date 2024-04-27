namespace Identity.Requests;

internal sealed record ForgotPasswordRequest(string Email, string ResetUrl);