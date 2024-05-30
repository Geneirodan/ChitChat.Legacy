using Ardalis.Specification;
using Common.Abstractions;
using Elastic.Clients.Elasticsearch;
using Mapster;
using Messages.Queries.Domain.Entities;
using Messages.Queries.Domain.Interfaces;

namespace Messages.Queries.Infrastructure.Elastic;

public sealed class MessageRepository(ElasticsearchClient client) : IMessageRepository
{
    private static readonly IndexName IndexName = Indices.Messages;

    public async Task<Message?> FindAsync(
        ISingleResultSpecification<Message> specification,
        CancellationToken cancellationToken = default
    ) => await GetValue(specification, cancellationToken);

    private async Task<Message?> GetValue(ISpecification<Message> specification, CancellationToken cancellationToken)
    {
        if (specification is not IGetByIdSpecification<Message> getByIdSpecification)
            throw new InvalidOperationException("ElasticRepository doesn't allow non IGetById specifications");

        var response = await client.GetAsync<Message>(IndexName, getByIdSpecification.Id, cancellationToken);
        return response.Source;
    }

    public async Task<TResult?> FindAsync<TResult>(ISingleResultSpecification<Message, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var response = await GetValue(specification, cancellationToken);
        return response.Adapt<TResult>();
    }

    public Task AddAsync(Message entity, CancellationToken cancellationToken = default)
        => UpdateAsync(entity, cancellationToken);

    public async Task UpdateAsync(Message entity, CancellationToken cancellationToken = default) =>
        await client.IndexAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default) =>
        await client.DeleteAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}

public abstract class ElasticRepository<TEntity>(ElasticsearchClient client)
    : IRepository<TEntity> where TEntity : class, Common.Abstractions.IEntity<Guid>
{
    protected abstract IndexName IndexName { get; }

    public async Task<TEntity?> FindAsync(
        ISingleResultSpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) => await GetValue(specification, cancellationToken);

    public async Task<TResult?> FindAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default)
    {
        var response = await GetValue(specification, cancellationToken);
        return response.Adapt<TResult>();
    }

    private async Task<TEntity?> GetValue(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        if (specification is not IGetByIdSpecification<TEntity> getByIdSpecification)
            throw new InvalidOperationException("ElasticRepository doesn't allow non IGetById specifications");
        var response = await client.GetAsync<TEntity>(IndexName, getByIdSpecification.Id, cancellationToken);
        return response.Source;
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => UpdateAsync(entity, cancellationToken);

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await client.IndexAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await client.DeleteAsync(entity, IndexName, cancellationToken).ConfigureAwait(false);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}