using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

public sealed class MessageDeletedEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageDeletedEvent>(endpoint);