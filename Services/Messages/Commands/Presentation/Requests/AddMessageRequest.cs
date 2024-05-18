namespace Messages.Commands.Presentation.Requests;

public sealed record AddMessageRequest(string Content, DateTime Timestamp, Guid ReceiverId);
