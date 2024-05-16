using Common.DDD;

namespace Messages.Commands.Domain.Events;

public sealed record MessageEditedEvent(Guid Id, string Content) : DomainEvent(Id);