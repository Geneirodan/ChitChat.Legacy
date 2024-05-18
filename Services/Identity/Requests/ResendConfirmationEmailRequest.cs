namespace Identity.Requests;


internal sealed record ResendConfirmationEmailRequest(string Email, string ConfirmUrl);

