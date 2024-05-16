using Messages.Commands.Domain.Events;

namespace Messages.Commands.Infrastructure.Marten.Aggregates;

internal sealed class Message : Domain.Message
{
    public void Apply(MessageCreatedEvent @event) => (Id, Content, SendTime, SenderId, ReceiverId) = @event;
    public void Apply(MessageEditedEvent @event) => Content = @event.Content;
    public void Apply(MessageReadEvent _) => IsRead = true;
    public void Apply(MessageDeletedEvent _) => IsDeleted = true;
}