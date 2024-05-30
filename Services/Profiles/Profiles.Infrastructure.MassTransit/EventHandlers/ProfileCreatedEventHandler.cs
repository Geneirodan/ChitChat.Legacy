using Profiles.Contracts;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileCreatedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.CreatedEvent, ProfileCreatedEvent>(endpoint);