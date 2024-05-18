using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces;
using Profiles.Application.ViewModels;
using Profiles.Domain;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record AddProfileCommand(string FirstName, string LastName) : IRequest<Result<ProfileViewModel?>>
{
    public sealed class Handler(IUser user, IProfileRepository repository, IPublisher publisher)
        : IRequestHandler<AddProfileCommand, Result<ProfileViewModel?>>
    {
        public async Task<Result<ProfileViewModel?>> Handle(
            AddProfileCommand request,
            CancellationToken cancellationToken
        )
        {
            var (firstName, lastName) = request;

            if (user.Id is null)
                return ErrorResults.Unauthorized();
            
            var id = user.Id.Value;

            var (profile, @event) = Profile.CreateInstance(id, firstName, lastName);

            await repository.AddAsync(profile, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return await repository.GetModelByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }
    }

    public sealed class Validator : AbstractValidator<AddProfileCommand>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.LastName).MaximumLength(64);
        }
    }
}