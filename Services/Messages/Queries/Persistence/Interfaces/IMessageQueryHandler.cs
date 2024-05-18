using Common.Other;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Filters;

namespace Messages.Queries.Persistence.Interfaces;

public interface IMessageQueryHandler
{
    Task<PaginatedList<Message>> QueryAsync(MessagesFilter filter, CancellationToken cancellationToken = default);
}