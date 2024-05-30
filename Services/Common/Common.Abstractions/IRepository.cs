using Ardalis.Specification;

namespace Common.Abstractions;

public interface IRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    Task<TResult?> FindAsync<TResult>(
        ISingleResultSpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> FindAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}