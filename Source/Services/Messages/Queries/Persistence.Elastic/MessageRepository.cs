using Elastic.Clients.Elasticsearch;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.Elastic;

public class MessageRepository(ElasticsearchClient client) : IMessageRepository
{
    private static readonly IndexName IndexName = Indices.Messages;

    public async Task<Message?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync<Message>(IndexName, id, cancellationToken).ConfigureAwait(false);
        return response.Source;
    }

    public Task AddAsync(Message entity, CancellationToken cancellationToken = default)
        => UpdateAsync(entity, cancellationToken);

    public async Task UpdateAsync(Message entity, CancellationToken cancellationToken = default) => 
        await client.IndexAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default) => 
        await client.DeleteAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}