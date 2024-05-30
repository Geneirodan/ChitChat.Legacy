namespace Profiles.Domain.ViewModels;

public sealed record ContactViewModel(Guid Id, string? FirstName, string? LastName, Guid UserId, Guid ProfileId);