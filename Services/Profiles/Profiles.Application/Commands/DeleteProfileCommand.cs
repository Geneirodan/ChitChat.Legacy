using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record DeleteProfileCommand : IRequest<Result>;

public sealed class DeleteProfileCommandHandler(IUser user, IProfilesRepository repository, IPublisher publisher) 
    : IRequestHandler<DeleteProfileCommand, Result>
{
    public async Task<Result> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetByIdSpecification<Profile>(user.Id!.Value);
        var profile = await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
        
        if(profile is null)
            return Result.NotFound();
        
        var @event = profile.Delete();
        
        await repository.DeleteAsync(profile, cancellationToken).ConfigureAwait(false);
        
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
        
        return Result.Success();
    }
}