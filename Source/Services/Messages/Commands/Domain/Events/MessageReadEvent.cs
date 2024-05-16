using Common.DDD;

namespace Messages.Commands.Domain.Events;

public sealed record MessageReadEvent(Guid Id) : DomainEvent(Id);