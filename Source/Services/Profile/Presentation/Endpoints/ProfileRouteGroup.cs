using Application.Commands;
using Application.Queries;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Requests;

namespace Presentation.Endpoints;

public static class ProfileRouteGroup
{
    public static void MapProfile(this IEndpointRouteBuilder app, string prefix)
    {
        var group = app.MapGroup(prefix).WithTags(prefix).RequireAuthorization();
        group.MapGet(string.Empty, GetProfileById);
        group.MapPost(string.Empty, AddProfile);
        group.MapPut(string.Empty, EditProfile);
        group.MapPatch("Avatar", SetAvatar).DisableAntiforgery();
        group.MapDelete(string.Empty, DeleteProfile);
    }

    private static async Task<IResult> GetProfileById(
        [FromServices] IMediator mediator
    )
    {
        var query = new GetProfileQuery();
        return await mediator.Send(query) is { } viewModel
            ? TypedResults.Ok(viewModel)
            : TypedResults.NotFound();
    }

    private static async Task<IResult> AddProfile(
        [FromBody] AddProfileRequest request,
        [FromServices] IMediator mediator)
    {
        var (firstName, lastName) = request;
        var command = new AddProfileCommand(firstName, lastName);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status201Created);
    }

    private static async Task<IResult> EditProfile(
        [FromBody] EditProfileRequest request,
        [FromServices] IMediator mediator
    )
    {
        var (firstName, lastName, bio) = request;
        var command = new EditProfileCommand(firstName, lastName, bio);
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }

    private static async Task<IResult> DeleteProfile(
        [FromServices] IMediator mediator
    )
    {
        var command = new DeleteProfileCommand();
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }

    private static async Task<IResult> SetAvatar(
        [FromForm] IFormFile file,
        [FromServices] IMediator mediator
    )
    {
        //TODO: Implement image storage
        var command = new SetAvatarCommand("");
        var result = await mediator.Send(command).ConfigureAwait(false);
        return result.ResultToResponse(StatusCodes.Status202Accepted);
    }
}