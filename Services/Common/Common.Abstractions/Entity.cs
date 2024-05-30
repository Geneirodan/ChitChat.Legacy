namespace Common.Abstractions;

public abstract class Entity<TKey> : IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; protected set; } = default!;
}