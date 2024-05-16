using Common.DDD;

namespace Messages.Commands.Domain.Events;

public sealed record MessageCreatedEvent(
    Guid Id,
    string Content,
    DateTime SendTime,
    Guid SenderId,
    Guid ReceiverId
) : DomainEvent(Id);