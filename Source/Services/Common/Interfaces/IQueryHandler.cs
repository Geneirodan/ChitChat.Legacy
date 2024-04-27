using AutoFilterer.Abstractions;

namespace Common.Interfaces;

// ReSharper disable once UnusedTypeParameter
public interface IQueryHandler<TEntity, TKey> where TEntity : class
{
    Task<IEnumerable<T>> QueryAsync<T>(CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> QueryAsync<T>(IFilter filter, CancellationToken cancellationToken = default);
    
    Task<PaginatedList<T>> QueryAsync<T>(IPaginationFilter filter, CancellationToken cancellationToken = default);
}