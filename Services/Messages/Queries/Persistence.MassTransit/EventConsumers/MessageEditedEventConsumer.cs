using MassTransit;
using Messages.Contracts;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

public sealed class MessageEditedEventConsumer(IMessageRepository repository) : IConsumer<MessageEditedEvent>
{

    public async Task Consume(ConsumeContext<MessageEditedEvent> context)
    {
        var (id, content, _, _) = context.Message;
        
        var message = await repository.FindAsync(id).ConfigureAwait(false);

        if (message is null) return;

        message.Content = content;
        
        await repository.UpdateAsync(message).ConfigureAwait(false);
        await repository.SaveChangesAsync().ConfigureAwait(false);
    }
}