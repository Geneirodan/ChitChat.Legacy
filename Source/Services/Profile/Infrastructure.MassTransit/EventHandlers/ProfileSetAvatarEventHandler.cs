using Profiles.Domain;
using Profiles.Contracts;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public sealed class ProfileSetAvatarEventHandler(IPublishEndpoint endpoint)
    : IntegrationEventHandler<Profile.AvatarUrlSetEvent, ProfileAvatarUrlSetEvent>(endpoint);