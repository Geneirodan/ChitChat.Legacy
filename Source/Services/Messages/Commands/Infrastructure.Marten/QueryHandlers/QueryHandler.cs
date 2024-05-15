

// public class QueryHandler<TEntity, TKey>(IQuerySession session) 
//     : IQueryHandler<TEntity, TKey> 
//     where TEntity : Aggregate<TKey>
// {
//     public Task<IEnumerable<T>> QueryAsync<T>(CancellationToken cancellationToken = default) =>
//         Task.FromResult<IEnumerable<T>>(session.Query<T>());
//
//     public async Task<PaginatedList<T>> QueryAsync<T, TFilterType>(TFilterType filter, CancellationToken cancellationToken = default)
//     {
//         var entities = session.Query<T>().ApplyFilterWithoutPagination(filter);
//         var paged = entities.ToPaged(filter.Page, filter.PerPage);
//
//         var count = await entities.CountAsync(cancellationToken);
//         return new PaginatedList<T>(paged, count);
//     }
//
//     public Task<IEnumerable<T>> QueryAsync<T>(IFilter filter, CancellationToken cancellationToken = default) =>
//         Task.FromResult<IEnumerable<T>>(session.Query<T>().ApplyFilter(filter))=;
// }