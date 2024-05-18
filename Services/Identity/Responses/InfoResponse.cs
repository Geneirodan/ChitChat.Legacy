namespace Identity.Responses;

internal sealed record InfoResponse(string Id, string Email, bool IsEmailConfirmed);