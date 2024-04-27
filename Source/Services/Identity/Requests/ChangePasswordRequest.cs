namespace Identity.Requests;

internal sealed record ChangePasswordRequest(string NewPassword, string OldPassword);