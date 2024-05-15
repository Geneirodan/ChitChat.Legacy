using Common.Interfaces;
using Common.MediatR.Attributes;
using FluentResults;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Contracts.IntegrationEvents;
using static System.Net.HttpStatusCode;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record ReadMessageCommand(Guid Id) : IRequest<Result>;

// ReSharper disable once UnusedType.Global
public sealed class ReadMessageHandler(IMessageRepository repository, IUser user, IPublisher publisher) : IRequestHandler<ReadMessageCommand, Result>
{
    public async Task<Result> Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await repository.FindAsync(request.Id, cancellationToken);

        if (message is null)
            return Result.Fail(nameof(NotFound));
        
        if (message.ReceiverId != user.Id)
            return Result.Fail(nameof(Forbidden));

        message.ReadMessage();
        
        await repository.UpdateAsync(message, cancellationToken);
        
        var @event = new MessageReadEvent(message.Id, message.SenderId, message.ReceiverId);
        await publisher.Publish(@event, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        return Result.Ok();
    }
}