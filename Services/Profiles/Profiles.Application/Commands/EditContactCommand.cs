using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Profiles.Domain.Aggregates;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record EditContactCommand(Guid Id, string? FirstName, string? LastName) : IRequest<Result>
{

    public sealed class Handler(IUser user, IContactsRepository repository, IPublisher publisher)
        : IRequestHandler<EditContactCommand, Result>
    {
        public async Task<Result> Handle(EditContactCommand request, CancellationToken cancellationToken)
        {
            var (id, firstName, lastName) = request;
            
            var spec = new GetByIdSpecification<Contact>(request.Id);
            var contact = await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
            //var contact = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

            if (contact is null)
                return Result.NotFound();

            if (contact.UserId != user.Id)
                return Result.Forbidden();

            var @event = contact.Edit(firstName, lastName);

            await repository.UpdateAsync(contact, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Success();
        }
    }

    public sealed class Validator : AbstractValidator<EditContactCommand>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.LastName).MaximumLength(64);
        }
    }
}