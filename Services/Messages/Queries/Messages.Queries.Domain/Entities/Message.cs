using Common.Abstractions;

namespace Messages.Queries.Domain.Entities;

public sealed class Message : IEntity<Guid>
{
    public Guid Id { get; init; }
    public string Content { get; set; } = null!;
    public bool IsRead { get; private set; }
    public DateTime SendTime { get; init; }
    public Guid SenderId { get; init; }
    public Guid ReceiverId { get; init; }
    public void Read() => IsRead = true;
}