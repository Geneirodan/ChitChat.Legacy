using Application.Interfaces;
using Common.Interfaces;
using Common.MediatR.Attributes;
using Domain;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Commands;

[Authorize]
public sealed record AddProfileCommand(string FirstName, string LastName) : IRequest<Result<Profile?>>;

public sealed class AddProfileCommandHandler(IUser user, IProfileRepository repository, IPublisher publisher) 
    : IRequestHandler<AddProfileCommand, Result<Profile?>>
{
    public async Task<Result<Profile?>> Handle(AddProfileCommand request, CancellationToken cancellationToken)
    {
        var (firstName, lastName) = request;
        
        var id = user.Id!.Value;
        
        var (profile, @event) = Profile.CreateInstance(id, firstName, lastName);
        
        await repository.AddAsync(profile, cancellationToken).ConfigureAwait(false);
        
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);
        
        var created = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

        return Result.Ok(created);
    }
}

public sealed class AddProfileCommandValidator : AbstractValidator<AddProfileCommand>
{
    public AddProfileCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).MaximumLength(64);
    }
}