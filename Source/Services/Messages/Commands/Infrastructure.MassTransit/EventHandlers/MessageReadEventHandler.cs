using MassTransit;
using Messages.Contracts.IntegrationEvents;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageReadEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageReadEvent>(endpoint);