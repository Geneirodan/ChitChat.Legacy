using Common;
using Mapster;
using MediatR;
using Messages.Queries.Business.Queries;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Presentation.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Queries.Presentation.Endpoints;

public static class MessageRouteGroup
{
    public static void MapMessages(this IEndpointRouteBuilder app, string prefix) => 
        app.MapGet(prefix, GetMessages).WithTags(nameof(Message)).RequireAuthorization();

    private static async Task<Ok<PaginatedList<Message>>> GetMessages(
        [AsParameters] GetMessagesRequest request,
        [FromServices] ISender mediator)
    {
        var query = request.Adapt<GetMessagesQuery>();
        return TypedResults.Ok(await mediator.Send(query).ConfigureAwait(false));
    }
}