using Common.Abstractions;

namespace Common.Domain;

public abstract class Aggregate : Entity<Guid>
{

    public int Version { get; protected set; }
    
    public bool IsDeleted { get; protected set; }

    [NonSerialized]
    private readonly Queue<DomainEvent> _uncommittedEvents = new();

    public IEnumerable<DomainEvent> DequeueUncommittedEvents()
    {
        while (_uncommittedEvents.TryDequeue(out var result))
            yield return result;
    }

    protected void Enqueue(DomainEvent @event) => _uncommittedEvents.Enqueue(@event);
}