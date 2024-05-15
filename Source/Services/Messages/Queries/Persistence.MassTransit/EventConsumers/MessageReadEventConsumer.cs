using MassTransit;
using Messages.Contracts.IntegrationEvents;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

// ReSharper disable once UnusedType.Global
public class MessageReadEventConsumer(IMessageRepository repository) : IConsumer<MessageReadEvent>
{

    public async Task Consume(ConsumeContext<MessageReadEvent> context)
    {
        var message = await repository.FindAsync(context.Message.Id);

        if (message is null) return;

        message.Read();

        await repository.UpdateAsync(message);
        await repository.SaveChangesAsync();
    }
}