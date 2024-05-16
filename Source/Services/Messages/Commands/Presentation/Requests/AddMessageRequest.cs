namespace Messages.Commands.Presentation.Requests;

internal sealed record AddMessageRequest(string Content, DateTime Timestamp, Guid ReceiverId);
