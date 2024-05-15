namespace Common.DomainDriven;

public abstract class Aggregate<TKey>
{
    public TKey Id { get; protected set; } = default!;

    public int Version { get; protected set; }
    
    public bool IsDeleted { get; set; }

    [NonSerialized]
    private readonly Queue<object> _uncommittedEvents = new();

    public object[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = _uncommittedEvents.ToArray();

        _uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    protected void Enqueue(object @event) => _uncommittedEvents.Enqueue(@event);
}