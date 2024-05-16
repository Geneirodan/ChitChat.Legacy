using Common.Other;

namespace Contracts;


public sealed record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : IntegrationEvent(Id);