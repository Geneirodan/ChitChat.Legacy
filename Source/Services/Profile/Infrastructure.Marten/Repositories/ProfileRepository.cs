using Application;
using Application.Interfaces;
using Infrastructure.Marten.Aggregates;

namespace Infrastructure.Marten.Repositories;

public sealed class ProfileRepository(IDocumentSession documentSession) : IProfileRepository
{
    public async Task<ProfileViewModel?> GetModelById(Guid id, CancellationToken cancellationToken = default) => 
        await documentSession.Query<ProfileViewModel>().FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
    
    public async Task<Domain.Profile?> FindAsync(Guid id, CancellationToken cancellationToken = default)=>
        await documentSession.Events.AggregateStreamAsync<Profile>(id, token: cancellationToken).ConfigureAwait(false);

    public Task AddAsync(Domain.Profile aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        documentSession.Events.StartStream<Profile>(aggregate.Id, events);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Domain.Profile aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        documentSession.Events.Append(aggregate.Id, expectedVersion, events);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Domain.Profile aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.DequeueUncommittedEvents();
        var expectedVersion = aggregate.Version + events.Length;
        documentSession.Events.Append(aggregate.Id, expectedVersion, events);
        documentSession.Events.ArchiveStream(aggregate.Id);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await documentSession.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}