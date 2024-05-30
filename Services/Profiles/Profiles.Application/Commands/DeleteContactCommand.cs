using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using MediatR;
using Profiles.Domain.Aggregates;
using Profiles.Application.Interfaces;

namespace Profiles.Application.Commands;

[Authorize]
public sealed record DeleteContactCommand(Guid Id) : IRequest<Result>
{

    public sealed class Handler(IUser user, IContactsRepository repository, IPublisher publisher)
        : IRequestHandler<DeleteContactCommand, Result>
    {
        public async Task<Result> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetByIdSpecification<Contact>(request.Id);
            var contact = await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);
            //var contact = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (contact is null)
                return Result.NotFound();

            if (contact.UserId != user.Id)
                return Result.Forbidden();

            var @event = contact.Delete();

            await repository.DeleteAsync(contact, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Success();
        }
    }
}