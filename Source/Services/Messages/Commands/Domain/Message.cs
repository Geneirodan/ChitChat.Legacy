using Common.DDD;
using Messages.Commands.Domain.Events;

namespace Messages.Commands.Domain;

public class Message : Aggregate
{
    public string Content { get; protected set; } = null!;

    public bool IsRead { get; protected set; }

    public DateTime SendTime { get; protected set; }

    public Guid SenderId { get; protected set; }

    public Guid ReceiverId { get; protected set; }

    public static (Message message, MessageCreatedEvent @event) CreateInstance(Guid id, string content,
        DateTime sendTime, Guid senderId, Guid receiverId)
    {
        var message = new Message
        {
            Id = id,
            Content = content,
            SendTime = sendTime,
            SenderId = senderId,
            ReceiverId = receiverId
        };
        var @event = new MessageCreatedEvent(
            message.Id,
            message.Content,
            message.SendTime,
            message.SenderId,
            message.ReceiverId
        );
        message.Enqueue(@event);
        return (message, @event);
    }


    public MessageEditedEvent Edit(string content)
    {
        Content = content;
        var @event = new MessageEditedEvent(Id, Content);
        Enqueue(@event);
        return @event;
    }


    public MessageReadEvent Read()
    {
        IsRead = true;
        var @event = new MessageReadEvent(Id);
        Enqueue(@event);
        return @event;
    }


    public MessageDeletedEvent Delete()
    {
        IsDeleted = true;
        var @event = new MessageDeletedEvent(Id);
        Enqueue(@event);
        return @event;
    }

}