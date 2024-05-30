using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Common.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Common.EFCore;

public class EntityFrameworkReadRepository<TEntity>(DbContext context)
    : IReadRepository<TEntity> where TEntity : class, Abstractions.IEntity<Guid>
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TResult?> GetAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default) =>
        await _dbSet.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity?> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default) =>
        await _dbSet.WithSpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<TResult?>> GetAllAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        CancellationToken cancellationToken = default) =>
        await _dbSet.WithSpecification(specification).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TEntity?>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default) =>
        await _dbSet.WithSpecification(specification).ToListAsync(cancellationToken);

    public async Task<PaginatedList<TResult>> GetAllPaginatedAsync<TResult>(
        ISpecification<TEntity, TResult> specification,
        int page, int perPage,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.WithSpecification(specification);

        var items = await query.Skip((page - 1) * perPage).Take(perPage).ToArrayAsync(cancellationToken);
        var count = await query.CountAsync(cancellationToken);
        return new PaginatedList<TResult>(items, page, perPage, count);
    }

    public async Task<PaginatedList<TEntity>> GetAllPaginatedAsync(
        ISpecification<TEntity> specification,
        int page, int perPage,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.WithSpecification(specification);

        var items = await query.Skip((page - 1) * perPage).Take(perPage).ToArrayAsync(cancellationToken);
        var count = await query.CountAsync(cancellationToken);
        return new PaginatedList<TEntity>(items, page, perPage, count);
    }
}