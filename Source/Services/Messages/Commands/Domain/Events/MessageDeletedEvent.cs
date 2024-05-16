using Common.DDD;

namespace Messages.Commands.Domain.Events;

public sealed record MessageDeletedEvent(Guid Id) : DomainEvent(Id);