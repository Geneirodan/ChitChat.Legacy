namespace Identity.Requests;

internal sealed record ChangeEmailRequest(string Email, string ReturnUrl);