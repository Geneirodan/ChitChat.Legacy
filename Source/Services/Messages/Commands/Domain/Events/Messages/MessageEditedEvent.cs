using MediatR;

namespace Messages.Commands.Domain.Events.Messages;

public record MessageEditedEvent(string Content) : INotification;