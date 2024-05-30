using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;
using Profiles.Domain.Aggregates;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record AddProfileCommand(string FirstName, string LastName) : IRequest<Result<ProfileViewModel?>>
{
    public sealed class Handler(IUser user, IProfilesRepository repository, IPublisher publisher)
        : IRequestHandler<AddProfileCommand, Result<ProfileViewModel?>>
    {
        public async Task<Result<ProfileViewModel?>> Handle(
            AddProfileCommand request,
            CancellationToken cancellationToken
        )
        {
            var (firstName, lastName) = request;

            if (user.Id is null)
                return Result.Unauthorized();
            
            var id = user.Id.Value;

            var (profile, @event) = Profile.CreateInstance(id, firstName, lastName);

            await repository.AddAsync(profile, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            var spec = new GetByIdSpecification<Profile, ProfileViewModel>(id);
            return await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
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