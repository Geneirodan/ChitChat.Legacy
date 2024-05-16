using Common.DDD;

namespace Domain.Events;

public sealed record ProfileDeletedEvent(Guid Id) : DomainEvent(Id);