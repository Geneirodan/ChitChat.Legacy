using Ardalis.Specification;
using Common;
using Messages.Queries.Domain.Entities;

namespace Messages.Queries.Domain.Interfaces;

public interface IMessageQueryHandler
{
    Task<PaginatedList<Message>> QueryAsync(ISpecification<Message> filter, int page, int perPage, CancellationToken cancellationToken = default);
}