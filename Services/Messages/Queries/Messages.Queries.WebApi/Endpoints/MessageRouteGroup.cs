using Ardalis.Result.AspNetCore;
using Mapster;
using MediatR;
using Messages.Queries.Application.Queries;
using Messages.Queries.Domain.Entities;
using Messages.Queries.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Queries.WebApi.Endpoints;

internal static class MessageRouteGroup
{
    public static void MapMessages(this IEndpointRouteBuilder app, string prefix) =>
        app.MapGet(prefix, GetMessages).WithTags(nameof(Message)).RequireAuthorization();

    private static async Task<IResult> GetMessages(
        [AsParameters] GetMessagesRequest request,
        [FromServices] ISender mediator
    )
    {
        var query = request.Adapt<GetMessagesQuery>();
        var result = await mediator.Send(query).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }
}