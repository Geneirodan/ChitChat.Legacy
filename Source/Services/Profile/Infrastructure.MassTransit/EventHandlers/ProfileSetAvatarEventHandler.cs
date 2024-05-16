namespace Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class ProfileSetAvatarEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileSetAvatarEvent, Contracts.ProfileSetAvatarEvent>(endpoint);