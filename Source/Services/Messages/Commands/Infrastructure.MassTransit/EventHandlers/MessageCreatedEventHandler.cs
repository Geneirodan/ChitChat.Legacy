using MassTransit;
using Messages.Contracts;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageCreatedEvent>(endpoint);