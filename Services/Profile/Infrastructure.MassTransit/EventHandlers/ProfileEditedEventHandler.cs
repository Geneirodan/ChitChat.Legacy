using Profiles.Domain;
using Profiles.Contracts;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileEditedEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.EditedEvent, ProfileEditedEvent>(endpoint);