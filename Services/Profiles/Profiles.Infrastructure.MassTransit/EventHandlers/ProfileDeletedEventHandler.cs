using Profiles.Contracts;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.DeletedEvent, ProfileDeletedEvent>(endpoint);