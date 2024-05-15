using MassTransit;
using Messages.Contracts.IntegrationEvents;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

// ReSharper disable once UnusedType.Global
public class MessageEditedEventConsumer(IMessageRepository repository) : IConsumer<MessageEditedEvent>
{

    public async Task Consume(ConsumeContext<MessageEditedEvent> context)
    {
        var (id, content, _, _) = context.Message;
        
        var message = await repository.FindAsync(id);

        if (message is null) return;

        message.Content = content;
        
        await repository.UpdateAsync(message);
        await repository.SaveChangesAsync();
    }
}