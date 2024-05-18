using System.ComponentModel.DataAnnotations;

namespace Messages.Queries.Persistence.Entities;

public sealed class Message
{
    public Guid Id { get; init; }
    [MaxLength(2048)]
    public string Content { get; set; } = null!;
    public bool IsRead { get; private set; }
    public DateTime SendTime { get; init; }
    public Guid SenderId { get; init; }
    public Guid ReceiverId { get; init; }
    public void Read() => IsRead = true;
}