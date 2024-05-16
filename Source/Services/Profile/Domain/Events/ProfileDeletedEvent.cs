using Common.DDD;

namespace Profile.Domain.Events;

public sealed record ProfileDeletedEvent(Guid Id) : DomainEvent(Id);