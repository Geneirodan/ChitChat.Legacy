using MassTransit;
using Messages.Contracts.IntegrationEvents;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageCreatedEvent>(endpoint);