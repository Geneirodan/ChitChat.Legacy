namespace Messages.Commands.Presentation.Requests;

public record AddMessageRequest(string Content, DateTime Timestamp, Guid ReceiverId);
