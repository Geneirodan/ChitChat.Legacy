using Common.Other;

namespace Profile.Contracts;


public sealed record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : IntegrationEvent(Id);