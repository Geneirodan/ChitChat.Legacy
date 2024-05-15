using MassTransit;
using Messages.Contracts.IntegrationEvents;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

// ReSharper disable once UnusedType.Global
public class MessageDeletedEventConsumer(IMessageRepository repository) : IConsumer<MessageDeletedEvent>
{

    public async Task Consume(ConsumeContext<MessageDeletedEvent> context)
    {
        var response = await repository.FindAsync(context.Message.Id);

        if (response is not null)
        {
            await repository.DeleteAsync(response);
            await repository.SaveChangesAsync();
        }
    }
}