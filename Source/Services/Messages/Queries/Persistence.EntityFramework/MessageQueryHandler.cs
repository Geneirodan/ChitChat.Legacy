using AutoFilterer.Extensions;
using Common;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Filters;
using Messages.Queries.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messages.Queries.Persistence.EntityFramework;

public class MessageQueryHandler(ApplicationContext context) : IMessageQueryHandler
{
    public async Task<PaginatedList<Message>> QueryAsync(MessagesFilter filter,
        CancellationToken cancellationToken = default)
    {
        var entities = context.Messages.AsNoTracking().ApplyFilterWithoutPagination(filter);
        var paged = entities.ToPaged(filter.Page, filter.PerPage);
        var count = await entities.CountAsync(cancellationToken);
        return new PaginatedList<Message>(paged, count);
    }
}