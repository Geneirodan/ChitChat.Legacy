using Common.Interfaces;
using Common.MediatR.Attributes;
using FluentResults;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain.Aggregates;
using Messages.Contracts.IntegrationEvents;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record AddMessageCommand(string Content, DateTime Timestamp, Guid ReceiverId)
    : IRequest<Result<MessageResponse?>>;

public sealed record MessageResponse(string Content, bool IsRead, DateTime SendTime);

// ReSharper disable once UnusedType.Global
public sealed class AddMessageCommandHandler(IMessageRepository repository, IUser user, IPublisher publisher)
    : IRequestHandler<AddMessageCommand, Result<MessageResponse?>>
{
    public async Task<Result<MessageResponse?>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();

        var (content, dateTime, receiverId) = request;

        var senderId = user.Id!.Value;
        var message = new Message(id, content, dateTime, senderId, receiverId);

        await repository.AddAsync(message, cancellationToken).ConfigureAwait(false);

        var @event = new MessageCreatedEvent(id, content, dateTime, senderId, receiverId);
        await publisher.Publish(@event, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var created = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

        var response = created is not null
            ? new MessageResponse(created.Content, created.IsRead, created.SendTime)
            : null;

        return Result.Ok(response);
    }
}

// ReSharper disable once UnusedType.Global
public class MessageAddValidator : AbstractValidator<AddMessageCommand>
{
    public MessageAddValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
        RuleFor(x => x.ReceiverId).NotEmpty();
        RuleFor(x => x.Timestamp).NotEmpty();
    }
}