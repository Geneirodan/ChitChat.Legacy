using Mapster;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;
using Message = Messages.Commands.Infrastructure.Marten.Aggregates.Message;

namespace Messages.Commands.Infrastructure.Marten.Repositories;

internal sealed class MessageRepository(IDocumentSession session) : IMessageRepository
{
    public async Task<Domain.Message?> FindAsync(Guid id, CancellationToken cancellationToken = default)=>
        await session.Events.AggregateStreamAsync<Message>(id, token: cancellationToken).ConfigureAwait(false);

    public Task AddAsync(Domain.Message aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        session.Events.StartStream<Message>(aggregate.Id, events);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Domain.Message aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        session.Events.Append(aggregate.Id, expectedVersion, events);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Domain.Message aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        session.Events.Append(aggregate.Id, expectedVersion, events);
        session.Events.ArchiveStream(aggregate.Id);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    public async Task<MessageViewModel?> GetModelByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await FindAsync(id, cancellationToken).ConfigureAwait(false);
        return message.Adapt<MessageViewModel>();
    }
}