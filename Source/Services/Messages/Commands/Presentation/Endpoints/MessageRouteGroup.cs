using Common.Extensions;
using MediatR;
using Messages.Commands.Application.Commands;
using Messages.Commands.Domain.Aggregates;
using Messages.Commands.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Messages.Commands.Presentation.Endpoints;

public static class MessageRouteGroup
{
    public static void MapMessages(this IEndpointRouteBuilder app, string prefix)
    {
        var group = app.MapGroup(prefix).WithTags(nameof(Message)).RequireAuthorization();
        group.MapPost(string.Empty, AddMessage);
        group.MapPatch("{id:guid}", EditMessage);
        group.MapPatch("{id:guid}/read", ReadMessage);
        group.MapDelete("{id:guid}", DeleteMessage);
    }

    private static async Task<IResult> AddMessage(
        [FromBody] AddMessageRequest request,
        [FromServices] IMediator mediator)
    {
        var (content, dateTime, receiverId) = request;
        var command = new AddMessageCommand(content, dateTime, receiverId);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status201Created);
    }

    private static async Task<IResult> EditMessage(
        [FromRoute] Guid id,
        [FromBody] EditMessageRequest request,
        [FromServices] IMediator mediator)
    {
        var command = new EditMessageCommand(id, request.Content);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }

    private static async Task<IResult> DeleteMessage(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator)
    {
        var command = new DeleteMessageCommand(id);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }

    private static async Task<IResult> ReadMessage(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator)
    {
        var command = new ReadMessageCommand(id);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }
}