using Common.Interfaces;
using Common.MediatR.Attributes;
using Common.Results;
using FluentResults;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Contracts;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record ReadMessageCommand(Guid Id) : IRequest<Result>;

// ReSharper disable once UnusedType.Global
public sealed class ReadMessageHandler(IMessageRepository repository, IUser user, IPublisher publisher)
    : IRequestHandler<ReadMessageCommand, Result>
{
    public async Task<Result> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (message is null)
            return ErrorResults.NotFound();

        if (message.ReceiverId != user.Id)
            return ErrorResults.Forbidden();

        message.Read();

        await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var @event = new MessageReadEvent(message.Id, message.SenderId, message.ReceiverId);
        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}