using AutoFilterer.Extensions;
using Common.Other;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Filters;
using Messages.Queries.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Messages.Queries.Persistence.EntityFramework;

public sealed class MessageQueryHandler(ApplicationContext context) : IMessageQueryHandler
{
    public async Task<PaginatedList<Message>> QueryAsync(MessagesFilter filter,
        CancellationToken cancellationToken = default)
    {
        var entities = context.Messages.AsNoTracking().ApplyFilterWithoutPagination(filter);
        var paged = entities.ToPaged(filter.Page, filter.PerPage);
        var count = await entities.CountAsync(cancellationToken).ConfigureAwait(false);
        return new PaginatedList<Message>(paged, count);
    }
}