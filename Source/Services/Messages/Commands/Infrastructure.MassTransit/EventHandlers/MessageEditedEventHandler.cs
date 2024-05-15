using MassTransit;
using Messages.Contracts.IntegrationEvents;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageEditedEvent>(endpoint);