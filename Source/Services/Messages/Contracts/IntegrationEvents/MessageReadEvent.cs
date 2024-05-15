namespace Messages.Contracts.IntegrationEvents;

[EntityName(nameof(MessageReadEvent))]
public sealed record MessageReadEvent(Guid Id, Guid SenderId, Guid ReceiverId) 
    : MessageEvent(Id, SenderId, ReceiverId);