using MassTransit;
using Messages.Contracts;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

internal sealed class MessageEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageEditedEvent>(endpoint);