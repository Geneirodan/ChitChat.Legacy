using AutoFilterer.Enums;
using Common.Abstractions;
using Common.Other;
using MediatR;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Filters;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Business.Queries;

public sealed record GetMessagesQuery(int Page, int PerPage, Guid ReceiverId, string Search)
    : IRequest<PaginatedList<Message>>;

public sealed class GetMessagesHandler(IMessageQueryHandler queryHandler, IUser user)
    : IRequestHandler<GetMessagesQuery, PaginatedList<Message>>
{
    public async Task<PaginatedList<Message>> Handle(
        GetMessagesQuery request,
        CancellationToken cancellationToken
    )
    {
        var (page, perPage, receiverId, search) = request;
        var filter = new MessagesFilter
        {
            Page = page,
            PerPage = perPage,
            SenderId = user.Id!.Value,
            ReceiverId = receiverId,
            Search = search,
            SortBy = Sorting.Descending,
            Sort = nameof(Message.SendTime)
        };
        return await queryHandler.QueryAsync(filter, cancellationToken).ConfigureAwait(false);
    }
}