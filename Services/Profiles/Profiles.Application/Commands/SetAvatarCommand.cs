using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record SetAvatarCommand(string AvatarUrl) : IRequest<Result>;

public sealed class SetAvatarCommandHandler(IUser user, IProfilesRepository repository, IPublisher publisher)
    : IRequestHandler<SetAvatarCommand, Result>
{
    public async Task<Result> Handle(SetAvatarCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetByIdSpecification<Profile>(user.Id!.Value);
        var profile = await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
        //var profile = await repository.FindAsync(user.Id!.Value, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Result.NotFound();

        var @event = profile.SetAvatar(request.AvatarUrl);

        await repository.UpdateAsync(profile, cancellationToken).ConfigureAwait(false);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

        return Result.Success();
    }
}