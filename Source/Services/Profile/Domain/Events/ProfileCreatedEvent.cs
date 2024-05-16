using Common.DDD;

namespace Domain.Events;

public sealed record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : DomainEvent(Id);