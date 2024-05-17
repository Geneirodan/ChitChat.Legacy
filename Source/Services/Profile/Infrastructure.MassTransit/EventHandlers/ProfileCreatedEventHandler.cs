using Profiles.Domain;
using Profiles.Contracts;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.CreatedEvent, ProfileCreatedEvent>(endpoint);