namespace Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class ProfileCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileCreatedEvent, Contracts.ProfileCreatedEvent>(endpoint);