using MassTransit;
using Messages.Contracts;

namespace Messages.SignalRNotifier.EventConsumers;

internal sealed class MessageDeletedEventConsumer(IPublishEndpoint endpoint) 
    : MessageEventConsumer<MessageDeletedEvent>(endpoint);