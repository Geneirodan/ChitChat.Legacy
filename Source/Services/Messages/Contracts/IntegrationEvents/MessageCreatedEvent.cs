namespace Messages.Contracts.IntegrationEvents;

[EntityName(nameof(MessageCreatedEvent))]
public sealed record MessageCreatedEvent(Guid Id, string Content, DateTime SendTime, Guid SenderId, Guid ReceiverId)
    : MessageEvent(Id, SenderId, ReceiverId);