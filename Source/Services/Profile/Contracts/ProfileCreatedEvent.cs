using Common;

namespace Contracts;

public record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : IntegrationEvent(Id);