namespace Messages.Commands.Domain;

public sealed record MessageViewModel(string Content, bool IsRead, DateTime SendTime);