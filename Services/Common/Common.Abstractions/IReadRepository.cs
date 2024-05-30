using Ardalis.Specification;

namespace Common.Abstractions;

public interface IReadRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    Task<TResult?> GetAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<TEntity?> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TResult?>> GetAllAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity?>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );

    Task<PaginatedList<TResult>> GetAllPaginatedAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        int page, int perPage,
        CancellationToken cancellationToken = default
    );

    Task<PaginatedList<TEntity>> GetAllPaginatedAsync(
        ISpecification<TEntity> specification,
        int page, int perPage,
        CancellationToken cancellationToken = default
    );
}