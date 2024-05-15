using MediatR;

namespace Messages.Commands.Domain.Events.Messages;

public record MessageCreatedEvent(Guid Id, string Content, DateTime SendTime, Guid SenderId, Guid ReceiverId) 
    : IRequest, INotification;