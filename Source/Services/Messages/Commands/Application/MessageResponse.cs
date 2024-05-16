namespace Messages.Commands.Application;

public sealed record MessageResponse(string Content, bool IsRead, DateTime SendTime);