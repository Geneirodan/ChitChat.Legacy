using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record EditProfileCommand(string FirstName, string LastName, string Bio) : IRequest<Result>;

public sealed class EditProfileCommandHandler(IUser user, IProfilesRepository repository, IPublisher publisher) 
    : IRequestHandler<EditProfileCommand, Result>
{
    public async Task<Result> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        var (firstName, lastName, bio) = request;
        var spec = new GetByIdSpecification<Profile>(user.Id!.Value);
        var profile = await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
        //var profile = await repository.FindAsync(user.Id!.Value, cancellationToken).ConfigureAwait(false);
        
        if(profile is null)
            return Result.NotFound();
        
        var @event = profile.Edit(firstName, lastName, bio);
        
        await repository.UpdateAsync(profile, cancellationToken).ConfigureAwait(false);
        
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
        
        return Result.Success();
    }
}

public sealed class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
{
    public EditProfileCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).MaximumLength(64);
        RuleFor(x => x.Bio).MaximumLength(256);
    }
}