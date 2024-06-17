using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

internal sealed class MessageCreatedEventConsumer(IPublishEndpoint endpoint)  
    : MessageEventConsumer<MessageCreatedEvent>(endpoint);