namespace Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class ProfileEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileEditedEvent, Contracts.ProfileEditedEvent>(endpoint);