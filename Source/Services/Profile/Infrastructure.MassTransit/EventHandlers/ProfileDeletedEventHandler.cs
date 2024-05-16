namespace Profile.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileDeletedEvent, Contracts.ProfileDeletedEvent>(endpoint);