using Common.Interfaces;
using Common.MediatR.Attributes;
using Common.Results;
using FluentResults;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Contracts;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record EditMessageCommand(Guid Id, string Content) : IRequest<Result>;

// ReSharper disable once UnusedType.Global
public sealed class EditMessageHandler(IMessageRepository repository, IUser user, IPublisher publisher)
    : IRequestHandler<EditMessageCommand, Result>
{
    public async Task<Result> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {
        var (id, content) = request;
        var message = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

        if (message is null)
            return ErrorResults.NotFound();

        if (message.SenderId != user.Id)
            return ErrorResults.Forbidden();

        message.Edit(content);

        await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var @event = new MessageEditedEvent(message.Id, message.Content, message.SenderId, message.ReceiverId);
        await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}

// ReSharper disable once UnusedType.Global
public class MessageEditValidator : AbstractValidator<EditMessageCommand>
{
    public MessageEditValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
    }
}