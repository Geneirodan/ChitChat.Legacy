namespace Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileEditedEvent, Contracts.ProfileEditedEvent>(endpoint);