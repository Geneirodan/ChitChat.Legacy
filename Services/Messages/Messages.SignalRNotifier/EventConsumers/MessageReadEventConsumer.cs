using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

internal sealed class MessageReadEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageReadEvent>(endpoint);