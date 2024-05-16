using MassTransit;
using Messages.Contracts;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class MessageDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<MessageDeletedEvent>(endpoint);