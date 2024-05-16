namespace Infrastructure.MassTransit.EventHandlers;

// ReSharper disable once UnusedType.Global
public class ProfileDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Domain.Events.ProfileDeletedEvent, Contracts.ProfileDeletedEvent>(endpoint);