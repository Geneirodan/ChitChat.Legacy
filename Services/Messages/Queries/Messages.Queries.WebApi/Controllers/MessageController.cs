using Common;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MediatR;
using Messages.Queries.Application.Queries;
using Messages.Queries.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Messages.Queries.WebApi.Controllers;

internal sealed class MessageController(ISender sender) : GraphController
{
    [Authorize]
    [QueryRoot("messages")]
    public async Task<PaginatedList<Message>> GetMessages(Guid receiverId,
        string search = "", int page = 1, int perPage = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetMessagesQuery(page, perPage, receiverId, search);
        return await sender.Send(query, cancellationToken).ConfigureAwait(false);
    }
}