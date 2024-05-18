using Common.Other;

namespace Profiles.Contracts;


public sealed record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : IntegrationEvent(Id);