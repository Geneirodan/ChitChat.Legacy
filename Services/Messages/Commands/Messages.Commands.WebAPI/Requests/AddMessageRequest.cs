namespace Messages.Commands.WebAPI.Requests;

public sealed record AddMessageRequest(string Content, DateTime Timestamp, Guid ReceiverId);
