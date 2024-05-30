using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record DeleteMessageCommand(Guid Id) : IRequest<Result>
{
    internal sealed class Handler(IMessageRepository repository, IUser user, IPublisher publisher)
        : IRequestHandler<DeleteMessageCommand, Result>
    {
        public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var specification = new GetByIdSpecification<Message>(request.Id);
            var message = await repository.FindAsync(specification, cancellationToken).ConfigureAwait(false);
            
            //var message = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

            if (message is null)
                return Result.NotFound();

            if (message.SenderId != user.Id && message.ReceiverId != user.Id)
                return Result.Forbidden();

            var @event = message.Delete();

            await repository.DeleteAsync(message, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Success();
        }
    }
}