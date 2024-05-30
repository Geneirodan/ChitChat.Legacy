using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Commands;
using Profiles.Application.Queries;
using Profiles.WebAPI.Requests;

namespace Profiles.WebAPI.Endpoints;

public static class ContactsRouteGroup
{
    private static async Task<IResult> GetContactById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var query = new GetContactByIdQuery(id);
        var result = await mediator.Send(query).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    public static void MapContacts(this IEndpointRouteBuilder app, string prefix)
    {
        var group = app.MapGroup(prefix).WithTags(prefix).RequireAuthorization();
        group.MapGet(string.Empty, GetContacts);
        group.MapGet("{id:guid}", GetContactById);
        group.MapPost(string.Empty, AddContact);
        group.MapPut("{id:guid}", EditContact);
        group.MapDelete("{id:guid}", DeleteContact);
    }

    private static async Task<IResult> GetContacts(
        [AsParameters] GetContactsRequest request,
        [FromServices] IMediator mediator
    )
    {
        var (page, perPage, search, sortBy, isDesc) = request;
        var query = new GetContactsQuery(page, perPage, search, sortBy, isDesc);
        var list = await mediator.Send(query).ConfigureAwait(false);
        return TypedResults.Ok(list);
    }

    private static async Task<IResult> AddContact(
        [FromBody] AddContactRequest request,
        [FromServices] IMediator mediator
    )
    {
        var (profileId, firstName, lastName) = request;
        var command = new AddContactCommand(profileId, firstName, lastName);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> EditContact(
        [FromRoute] Guid id,
        [FromBody] EditContactRequest request,
        [FromServices] IMediator mediator
    )
    {
        var (firstName, lastName) = request;
        var command = new EditContactCommand(id, firstName, lastName);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> DeleteContact(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var command = new DeleteContactCommand(id);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }
}