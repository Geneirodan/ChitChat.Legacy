using Common.Interfaces;
using Common.MediatR.Attributes;
using FluentResults;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Contracts.IntegrationEvents;
using static System.Net.HttpStatusCode;

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
        var message = await repository.FindAsync(id, cancellationToken);

        if (message is null)
            return Result.Fail(nameof(NotFound));

        if (message.SenderId != user.Id)
            return Result.Fail(nameof(Forbidden));

        message.EditMessage(content);

        await repository.UpdateAsync(message, cancellationToken);

        var @event = new MessageEditedEvent(message.Id, message.Content, message.SenderId, message.ReceiverId);
        await publisher.Publish(@event, cancellationToken);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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