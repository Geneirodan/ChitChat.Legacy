using MassTransit;
using Messages.Contracts;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

internal sealed class MessageCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageCreatedEvent>(endpoint);