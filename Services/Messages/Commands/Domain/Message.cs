using Common.DDD;

namespace Messages.Commands.Domain;

public class Message : Aggregate
{
    public string Content { get; protected set; } = null!;

    public bool IsRead { get; protected set; }

    public DateTime SendTime { get; protected set; }

    public Guid SenderId { get; protected set; }

    public Guid ReceiverId { get; protected set; }

    public static (Message message, CreatedEvent @event) CreateInstance(Guid id, string content,
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
        var @event = new CreatedEvent(
            message.Id,
            message.Content,
            message.SendTime,
            message.SenderId,
            message.ReceiverId
        );
        message.Enqueue(@event);
        return (message, @event);
    }


    public EditedEvent Edit(string content)
    {
        Content = content;
        var @event = new EditedEvent(Id, Content);
        Enqueue(@event);
        return @event;
    }


    public ReadEvent Read()
    {
        IsRead = true;
        var @event = new ReadEvent(Id);
        Enqueue(@event);
        return @event;
    }


    public DeletedEvent Delete()
    {
        IsDeleted = true;
        var @event = new DeletedEvent(Id);
        Enqueue(@event);
        return @event;
    }

    public sealed record CreatedEvent(
        Guid Id,
        string Content,
        DateTime SendTime,
        Guid SenderId,
        Guid ReceiverId
    ) : DomainEvent(Id);
    public sealed record EditedEvent(Guid Id, string Content) : DomainEvent(Id);
    public sealed record ReadEvent(Guid Id) : DomainEvent(Id);
    public sealed record DeletedEvent(Guid Id) : DomainEvent(Id);
}