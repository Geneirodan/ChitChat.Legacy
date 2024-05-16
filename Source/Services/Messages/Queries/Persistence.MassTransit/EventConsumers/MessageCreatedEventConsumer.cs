using MassTransit;
using Messages.Contracts;
using Messages.Queries.Persistence.Entities;
using Messages.Queries.Persistence.Interfaces;

namespace Messages.Queries.Persistence.MassTransit.EventConsumers;

// ReSharper disable once UnusedType.Global
public class MessageCreatedEventConsumer(IMessageRepository repository) : IConsumer<MessageCreatedEvent>
{
    public async Task Consume(ConsumeContext<MessageCreatedEvent> context)
    {
        var (id, content, sendTime, senderId, receiverId) = context.Message;

        var response = await repository.FindAsync(id).ConfigureAwait(false);

        if (response is not null) return;

        var message = new Message
        {
            Id = id, 
            Content = content, 
            SendTime = sendTime, 
            SenderId = senderId,
            ReceiverId = receiverId
        };

        await repository.AddAsync(message).ConfigureAwait(false);
        await repository.SaveChangesAsync().ConfigureAwait(false);
    }
}