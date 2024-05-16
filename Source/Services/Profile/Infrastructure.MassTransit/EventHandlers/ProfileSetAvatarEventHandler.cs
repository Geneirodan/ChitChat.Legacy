namespace Profile.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileSetAvatarEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileSetAvatarEvent, Contracts.ProfileSetAvatarEvent>(endpoint);