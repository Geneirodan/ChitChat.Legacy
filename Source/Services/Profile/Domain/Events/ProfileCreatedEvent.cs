using Common.DDD;

namespace Profile.Domain.Events;

public sealed record ProfileCreatedEvent(Guid Id, string FirstName, string LastName) : DomainEvent(Id);