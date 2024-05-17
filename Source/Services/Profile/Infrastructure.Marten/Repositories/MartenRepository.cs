using Common.Abstractions;
using Common.DDD;

namespace Profiles.Infrastructure.Marten.Repositories;

public class MartenRepository<TDomainAggregate, TMartenAggregate>(IDocumentSession documentSession) : IRepository<TDomainAggregate, Guid>
    where TDomainAggregate : Aggregate
    where TMartenAggregate : TDomainAggregate
{
    public async Task<TDomainAggregate?> FindAsync(Guid id, CancellationToken cancellationToken = default) => 
        await documentSession.Events.AggregateStreamAsync<TMartenAggregate>(id, token: cancellationToken).ConfigureAwait(false);

    public Task AddAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        documentSession.Events.StartStream<TMartenAggregate>(aggregate.Id, events);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        documentSession.Events.Append(aggregate.Id, expectedVersion, events);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        documentSession.Events.Append(aggregate.Id, expectedVersion, events);
        documentSession.Events.ArchiveStream(aggregate.Id);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        documentSession.SaveChangesAsync(cancellationToken);
}