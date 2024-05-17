namespace Profiles.Presentation.Requests;

public sealed record AddContactRequest(Guid ProfileId, string? FirstName, string? LastName);