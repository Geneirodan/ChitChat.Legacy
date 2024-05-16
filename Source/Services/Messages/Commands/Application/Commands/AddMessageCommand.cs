using Common.Abstractions;
using Common.Mediator.Attributes;
using FluentResults;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;
using Messages.Contracts;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record AddMessageCommand(string Content, DateTime Timestamp, Guid ReceiverId)
    : IRequest<Result<MessageResponse>>;

internal sealed class AddMessageCommandHandler(IMessageRepository repository, IUser user, IPublisher publisher)
    : IRequestHandler<AddMessageCommand, Result<MessageResponse>>
{
    public async Task<Result<MessageResponse>> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();

        var (content, dateTime, receiverId) = request;

        var senderId = user.Id!.Value;
        var (message, @event) = Message.CreateInstance(id, content, dateTime, senderId, receiverId);

        await repository.AddAsync(message, cancellationToken).ConfigureAwait(false);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

        var created = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

        //TODO: Choose right option
        if (created is null) 
            return Result.Ok();
        
        var response = new MessageResponse(created.Content, created.IsRead, created.SendTime);
        
        return Result.Ok(response);

    }
}

internal sealed class MessageAddValidator : AbstractValidator<AddMessageCommand>
{
    public MessageAddValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
        RuleFor(x => x.ReceiverId).NotEmpty();
        RuleFor(x => x.Timestamp).NotEmpty();
    }
}