using Profiles.Contracts;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.EditedEvent, ProfileEditedEvent>(endpoint);