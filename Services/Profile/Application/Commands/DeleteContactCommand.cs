using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using MediatR;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record DeleteContactCommand(Guid Id) : IRequest<Result>
{

    public sealed class Handler(IUser user, IContactRepository repository, IPublisher publisher)
        : IRequestHandler<DeleteContactCommand, Result>
    {
        public async Task<Result> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (contact is null)
                return ErrorResults.NotFound();

            if (contact.UserId != user.Id)
                return ErrorResults.Forbidden();

            var @event = contact.Delete();

            await repository.DeleteAsync(contact, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Ok();
        }
    }
}