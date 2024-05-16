using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

public sealed class MessageReadEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageReadEvent>(endpoint);