using MassTransit;
using Messages.Contracts.IntegrationEvents;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageDeletedEvent>(endpoint);