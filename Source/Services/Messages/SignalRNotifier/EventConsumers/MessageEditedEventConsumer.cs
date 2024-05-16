using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

public sealed class MessageEditedEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageEditedEvent>(endpoint);