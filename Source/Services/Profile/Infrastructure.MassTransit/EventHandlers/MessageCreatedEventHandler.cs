namespace Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileCreatedEvent, Contracts.ProfileCreatedEvent>(endpoint);