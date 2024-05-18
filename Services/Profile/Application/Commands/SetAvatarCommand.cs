using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record SetAvatarCommand(string AvatarUrl) : IRequest<Result>;

public sealed class SetAvatarCommandHandler(IUser user, IProfileRepository repository, IPublisher publisher)
    : IRequestHandler<SetAvatarCommand, Result>
{
    public async Task<Result> Handle(SetAvatarCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.FindAsync(user.Id!.Value, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return ErrorResults.NotFound();

        var @event = profile.SetAvatar(request.AvatarUrl);

        await repository.UpdateAsync(profile, cancellationToken).ConfigureAwait(false);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}