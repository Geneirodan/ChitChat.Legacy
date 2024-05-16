using Common.DomainDriven;
using Common.Interfaces;

namespace Messages.Commands.Infrastructure.Marten.Repositories;

public class Repository<TEntity>(IDocumentSession documentSession) : IRepository<TEntity, Guid>
    where TEntity : Aggregate
{
    public virtual Task<TEntity?> FindAsync(Guid id, CancellationToken cancellationToken = default) => 
        documentSession.Events.AggregateStreamAsync<TEntity>(id, token: cancellationToken);

    public virtual Task AddAsync(TEntity aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();

        documentSession.Events.StartStream<TEntity>(aggregate.Id, events);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(TEntity aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();

        var nextVersion = aggregate.Version + events.Length;

        documentSession.Events.Append(aggregate.Id, nextVersion, events);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();

        var nextVersion = aggregate.Version + events.Length;

        documentSession.Events.Append(aggregate.Id, nextVersion, events);
        
        documentSession.Events.ArchiveStream(aggregate.Id);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await documentSession.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}