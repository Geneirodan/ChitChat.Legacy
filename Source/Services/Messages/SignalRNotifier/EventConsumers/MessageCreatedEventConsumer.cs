using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

public sealed class MessageCreatedEventConsumer(IPublishEndpoint endpoint)  
    : MessageEventConsumer<MessageCreatedEvent>(endpoint);