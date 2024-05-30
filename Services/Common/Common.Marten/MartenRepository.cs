using Ardalis.Specification;
using Common.Abstractions;
using Common.Domain;
using Mapster;

// ReSharper disable once CheckNamespace
namespace Marten;

public class MartenRepository<TDomainAggregate, TMartenAggregate>(IDocumentSession session)
    : IRepository<TDomainAggregate>
    where TDomainAggregate : Aggregate
    where TMartenAggregate : TDomainAggregate
{
    public async Task<TResult?> FindAsync<TResult>(
        ISingleResultSpecification<TDomainAggregate, TResult> specification,
        CancellationToken cancellationToken = default
    )
    {
        var aggregate = await AggregateStreamAsync(specification, cancellationToken).ConfigureAwait(false);
        return aggregate.Adapt<TResult>();
    }

    public Task<TDomainAggregate?> FindAsync(
        ISingleResultSpecification<TDomainAggregate> specification,
        CancellationToken cancellationToken = default
    ) => AggregateStreamAsync(specification, cancellationToken);

    private async Task<TDomainAggregate?> AggregateStreamAsync(
        ISpecification<TDomainAggregate> specification,
        CancellationToken cancellationToken
    )
    {
        if (specification is not IGetByIdSpecification<TDomainAggregate> { Id: var id })
            throw new InvalidOperationException("MartenRepository doesn't allow non IGetById specifications");
        
        return await session.Events
            .AggregateStreamAsync<TMartenAggregate>(id, token: cancellationToken)
            .ConfigureAwait(false);
    }


    public Task AddAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        session.Events.StartStream<TMartenAggregate>(aggregate.Id, events);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents().ToArray();
        var expectedVersion = aggregate.Version + events.Length;
        session.Events.Append(aggregate.Id, expectedVersion, events);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TDomainAggregate aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents().ToArray();
        var expectedVersion = aggregate.Version + events.Length;
        session.Events.Append(aggregate.Id, expectedVersion, events);
        session.Events.ArchiveStream(aggregate.Id);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        session.SaveChangesAsync(cancellationToken);
}