using Common.Other;

namespace Profile.Contracts;


public sealed record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : IntegrationEvent(Id);