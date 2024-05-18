using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using FluentValidation;
using MediatR;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record EditContactCommand(Guid Id, string? FirstName, string? LastName) : IRequest<Result>
{

    public sealed class Handler(IUser user, IContactRepository repository, IPublisher publisher)
        : IRequestHandler<EditContactCommand, Result>
    {
        public async Task<Result> Handle(EditContactCommand request, CancellationToken cancellationToken)
        {
            var (id, firstName, lastName) = request;
            var contact = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

            if (contact is null)
                return ErrorResults.NotFound();

            if (contact.UserId != user.Id)
                return ErrorResults.Forbidden();

            var @event = contact.Edit(firstName, lastName);

            await repository.UpdateAsync(contact, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Ok();
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