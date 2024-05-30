using Ardalis.Result;
using Common.Abstractions;
using Common.MediatR.Attributes;
using FluentValidation;
using MediatR;
using Messages.Commands.Application.Interfaces;
using Messages.Commands.Domain;

namespace Messages.Commands.Application.Commands;

[Authorize]
public sealed record AddMessageCommand(string Content, DateTime Timestamp, Guid ReceiverId)
    : IRequest<Result<MessageViewModel?>>
{
    internal sealed class Handler(IMessageRepository repository, IUser user, IPublisher publisher)
        : IRequestHandler<AddMessageCommand, Result<MessageViewModel?>>
    {
        public async Task<Result<MessageViewModel?>> Handle(AddMessageCommand request,
            CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var (content, dateTime, receiverId) = request;
            
            if (user.Id is null) 
                return Result.Unauthorized();

            var (message, @event) = Message.CreateInstance(id, content, dateTime, user.Id.Value, receiverId);

            await repository.AddAsync(message, cancellationToken).ConfigureAwait(false);

            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await publisher.Publish(@event, cancellationToken).ConfigureAwait(false);

            var spec = new GetByIdSpecification<Message, MessageViewModel>(id);
            return await repository.FindAsync(spec, cancellationToken).ConfigureAwait(false);

        }
    }

    internal sealed class Validator : AbstractValidator<AddMessageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(2048);
            RuleFor(x => x.ReceiverId).NotEmpty();
            RuleFor(x => x.Timestamp).NotEmpty();
        }
    }
}