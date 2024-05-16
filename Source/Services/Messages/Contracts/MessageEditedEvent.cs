namespace Messages.Contracts;


public sealed record MessageEditedEvent(Guid Id, string Content, Guid SenderId, Guid ReceiverId) 
    : MessageEvent(Id, SenderId, ReceiverId);