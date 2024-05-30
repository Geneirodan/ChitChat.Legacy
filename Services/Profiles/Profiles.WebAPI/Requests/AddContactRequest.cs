namespace Profiles.WebAPI.Requests;

public sealed record AddContactRequest(Guid ProfileId, string? FirstName, string? LastName);