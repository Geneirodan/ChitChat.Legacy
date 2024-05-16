using Common.DomainDriven;
using Messages.Commands.Domain.Events.Messages;

namespace Messages.Commands.Domain.Aggregates;

public class Message() : Aggregate
{
    public string Content { get; private set; } = null!;

    public bool IsRead { get; private set; }

    public DateTime SendTime { get; private set; }

    public Guid SenderId { get; private set; }

    public Guid ReceiverId { get; private set; }

    public Message(Guid id, string content, DateTime sendTime, Guid senderId, Guid receiverId) : this()
    {
        var @event = new MessageCreatedEvent(id, content, sendTime, senderId, receiverId);
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageCreatedEvent @event) => (Id, Content, SendTime, SenderId, ReceiverId) = @event;

    public void Edit(string content)
    {
        var @event = new MessageEditedEvent(content);
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageEditedEvent @event) => Content = @event.Content;

    public void Read()
    {
        var @event = new MessageReadEvent();
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageReadEvent _) => IsRead = true;

    public void Delete()
    {
        var @event = new MessageDeletedEvent();
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageDeletedEvent _) => IsDeleted = true;
}