using Common.DomainDriven;

namespace Domain.Events;

public record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : DomainEvent(Id);