using MassTransit;
using Messages.Contracts;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

public sealed record MessageReadEventConsumer(IMessageRepository repository) : IConsumer<MessageReadEvent>
{

    public async Task Consume(ConsumeContext<MessageReadEvent> context)
    {
        var message = await repository.FindAsync(context.Message.Id).ConfigureAwait(false);

        if (message is null) return;

        message.Read();

        await repository.UpdateAsync(message).ConfigureAwait(false);
        await repository.SaveChangesAsync().ConfigureAwait(false);
    }
}