using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Commands;
using Profiles.Application.Queries;
using Profiles.WebAPI.Requests;

namespace Profiles.WebAPI.Endpoints;

public static class ProfileRouteGroup
{
    public static void MapProfile(this IEndpointRouteBuilder app, string prefix)
    {
        var group = app.MapGroup(prefix).WithTags(prefix).RequireAuthorization();
        group.MapGet(string.Empty, GetProfile);
        group.MapGet("{id:guid}", GetProfileById);
        group.MapPost(string.Empty, AddProfile);
        group.MapPut(string.Empty, EditProfile);
        group.MapPatch("Avatar", SetAvatar).DisableAntiforgery();
        group.MapDelete(string.Empty, DeleteProfile);
    }

    private static async Task<IResult> GetProfile(
        [FromServices] IMediator mediator
    )
    {
        var query = new GetProfileQuery();
        var result = await mediator.Send(query).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> GetProfileById(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var query = new GetProfileByIdQuery(id);
        var result = await mediator.Send(query).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> AddProfile(
        [FromBody] AddProfileRequest request,
        [FromServices] IMediator mediator)
    {
        var (firstName, lastName) = request;
        var command = new AddProfileCommand(firstName, lastName);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> EditProfile(
        [FromBody] EditProfileRequest request,
        [FromServices] IMediator mediator
    )
    {
        var (firstName, lastName, bio) = request;
        var command = new EditProfileCommand(firstName, lastName, bio);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> DeleteProfile(
        [FromServices] IMediator mediator
    )
    {
        var command = new DeleteProfileCommand();
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }

    private static async Task<IResult> SetAvatar(
        [FromForm] IFormFile file,
        [FromServices] IMediator mediator
    )
    {
        //TODO: Implement image storage
        var command = new SetAvatarCommand("");
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ToMinimalApiResult();
    }
}