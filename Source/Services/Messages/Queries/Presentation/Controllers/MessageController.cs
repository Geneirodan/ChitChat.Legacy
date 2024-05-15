using Common;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using MediatR;
using Messages.Queries.Business.Queries;
using Messages.Queries.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Messages.Queries.Presentation.Controllers;

public class MessageController(ISender sender) : GraphController
{
    [Authorize]
    [QueryRoot("messages")]
    public async Task<PaginatedList<Message>> GetMessages(Guid receiverId,
        string search = "", int page = 1, int perPage = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetMessagesQuery(page, perPage, receiverId, search);
        return await sender.Send(query, cancellationToken);
    }
}