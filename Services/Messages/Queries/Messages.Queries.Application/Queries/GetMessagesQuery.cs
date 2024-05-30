using Ardalis.Result;
using Ardalis.Specification;
using Common;
using Common.Abstractions;
using MediatR;
using Messages.Queries.Domain.Entities;
using Messages.Queries.Domain.Interfaces;

namespace Messages.Queries.Application.Queries;

public sealed record GetMessagesQuery(int Page, int PerPage, Guid ReceiverId, string Search)
    : IRequest<Result<PaginatedList<Message>>>
{
    public sealed class Specification : Specification<Message>
    {
        public Specification(string searchTerm, Guid senderId, Guid receiverId)
        {
            Query.Where(x => x.SenderId == senderId);
            Query.Where(x => x.ReceiverId == receiverId);
            Query.Search(x => x.Content, searchTerm);
        }
    }

    public sealed class Handler(IMessageQueryHandler queryHandler, IUser user)
        : IRequestHandler<GetMessagesQuery, Result<PaginatedList<Message>>>
    {
        public async Task<Result<PaginatedList<Message>>> Handle(
            GetMessagesQuery request,
            CancellationToken cancellationToken
        )
        {
            if (user is { Id: null })
                return Result.Unauthorized();
            var (page, perPage, receiverId, search) = request;
            var filter = new Specification(search, user.Id.Value, receiverId);
            return await queryHandler.QueryAsync(filter, page, perPage, cancellationToken).ConfigureAwait(false);
        }
    }
}