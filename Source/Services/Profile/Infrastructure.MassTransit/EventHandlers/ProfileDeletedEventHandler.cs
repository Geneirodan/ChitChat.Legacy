using Profiles.Domain;
using Profiles.Contracts;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileDeletedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.DeletedEvent, ProfileDeletedEvent>(endpoint);