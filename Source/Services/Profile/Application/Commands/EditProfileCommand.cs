using Application.Interfaces;
using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using FluentValidation;
using MediatR;


namespace Application.Commands;

[Authorize]
public sealed record EditProfileCommand(string FirstName, string LastName, string Bio) : IRequest<Result>;

public sealed class EditProfileCommandHandler(IUser user, IProfileRepository repository, IPublisher publisher) 
    : IRequestHandler<EditProfileCommand, Result>
{
    public async Task<Result> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        var (firstName, lastName, bio) = request;
        
        var profile = await repository.FindAsync(user.Id!.Value, cancellationToken).ConfigureAwait(false);
        
        if(profile is null)
            return ErrorResults.NotFound();
        
        var @event = profile.Edit(firstName, lastName, bio);
        
        await repository.UpdateAsync(profile, cancellationToken).ConfigureAwait(false);
        
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
        
        return Result.Ok();
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