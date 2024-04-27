namespace Identity.Requests;

internal sealed record RegisterRequest(string Username, string Email, string Password, string ConfirmUrl);