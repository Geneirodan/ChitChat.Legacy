namespace Messages.Contracts;


public sealed record MessageDeletedEvent(Guid Id, Guid SenderId, Guid ReceiverId) 
    : MessageEvent(Id, SenderId, ReceiverId);