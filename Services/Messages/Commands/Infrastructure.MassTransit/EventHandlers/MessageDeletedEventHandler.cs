using MassTransit;
using Messages.Contracts;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

internal sealed class MessageDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageDeletedEvent>(endpoint);