namespace Messages.Contracts;


public sealed record MessageReadEvent(Guid Id, Guid SenderId, Guid ReceiverId) 
    : MessageEvent(Id, SenderId, ReceiverId);