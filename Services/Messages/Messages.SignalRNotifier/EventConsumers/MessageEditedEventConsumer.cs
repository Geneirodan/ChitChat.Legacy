using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

internal sealed class MessageEditedEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageEditedEvent>(endpoint);