using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record EditMessageCommand(Guid Id, string Content) : IRequest<Result>
{
    internal sealed class Handler(IMessageRepository repository, IUser user, IPublisher publisher)
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

            var @event = message.Edit(content);

            await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.Ok();
        }
    }

    internal sealed class Validator : AbstractValidator<EditMessageCommand>
    {
        public Validator() => RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
    }
}