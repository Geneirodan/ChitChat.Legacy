using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;

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

            var specification = new GetByIdSpecification<Message>(id);
            var message = await repository.FindAsync(specification, cancellationToken).ConfigureAwait(false);
            
            //var message = await repository.FindAsync(id, cancellationToken).ConfigureAwait(false);

            if (message is null)
                return Result.NotFound();

            if (message.SenderId != user.Id)
                return Result.Forbidden();

            var @event = message.Edit(content);

            await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            return Result.NoContent();
        }
    }

    internal sealed class Validator : AbstractValidator<EditMessageCommand>
    {
        public Validator() => RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
    }
}