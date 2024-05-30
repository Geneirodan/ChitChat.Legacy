using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Profiles.Domain.Aggregates;
using Profiles.Domain.ViewModels;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record AddContactCommand(Guid ProfileId, string? FirstName, string? LastName)
    : IRequest<Result<ContactViewModel?>>
{
    public sealed class Handler(IUser user, IContactsRepository repository, IPublisher publisher)
        : IRequestHandler<AddContactCommand, Result<ContactViewModel?>>
    {
        public async Task<Result<ContactViewModel?>> Handle(AddContactCommand request,
            CancellationToken cancellationToken)
        {
            var (profileId, firstName, lastName) = request;

            var id = Guid.NewGuid();

            var (contact, @event) = Contact.CreateInstance(id, firstName, lastName, user.Id!.Value, profileId);

            await repository.AddAsync(contact, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            var spec = new GetByIdSpecification<Contact, ContactViewModel>(id);
            return await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
        }
    }

    public sealed class Validator : AbstractValidator<AddContactCommand>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).MaximumLength(64);
            RuleFor(x => x.LastName).MaximumLength(64);
        }
    }
}