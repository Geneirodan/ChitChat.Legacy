using Common.Other;

namespace Contracts;


public sealed record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : IntegrationEvent(Id);