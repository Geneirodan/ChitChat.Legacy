using Common.DomainDriven;
using Messages.Commands.Domain.Events.Messages;

namespace Messages.Commands.Domain.Aggregates;

public class Message() : Aggregate<Guid>
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

    public void Apply(MessageCreatedEvent @event)
    {
        Id = @event.Id;
        Content = @event.Content;
        SendTime = @event.SendTime;
        SenderId = @event.SenderId;
        ReceiverId = @event.ReceiverId;
    }

    public void EditMessage(string content)
    {
        var @event = new MessageEditedEvent(content);
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageEditedEvent @event) => Content = @event.Content;

    public void ReadMessage()
    {
        var @event = new MessageReadEvent();
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageReadEvent _) => IsRead = true;

    public void DeleteMessage()
    {
        var @event = new MessageDeletedEvent();
        Enqueue(@event);
        Apply(@event);
    }

    public void Apply(MessageDeletedEvent _) => IsDeleted = true;
}