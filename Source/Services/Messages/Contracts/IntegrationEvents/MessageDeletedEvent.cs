namespace Messages.Contracts.IntegrationEvents;

[EntityName(nameof(MessageDeletedEvent))]
public sealed record MessageDeletedEvent(Guid Id, Guid SenderId, Guid ReceiverId) 
    : MessageEvent(Id, SenderId, ReceiverId);