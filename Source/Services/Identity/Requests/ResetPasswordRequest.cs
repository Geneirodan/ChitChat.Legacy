namespace Identity.Requests;

internal sealed record ResetPasswordRequest(string Email, string ResetCode, string NewPassword);