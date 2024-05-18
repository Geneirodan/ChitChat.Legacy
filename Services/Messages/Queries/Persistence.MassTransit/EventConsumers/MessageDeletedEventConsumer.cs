using MassTransit;
using Messages.Contracts;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

public sealed class MessageDeletedEventConsumer(IMessageRepository repository) : IConsumer<MessageDeletedEvent>
{
    public async Task Consume(ConsumeContext<MessageDeletedEvent> context)
    {
        var response = await repository.FindAsync(context.Message.Id).ConfigureAwait(false);

        if (response is null) return;

        await repository.DeleteAsync(response).ConfigureAwait(false);
        await repository.SaveChangesAsync().ConfigureAwait(false);
    }
}