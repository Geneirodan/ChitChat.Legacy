using Common;

namespace Profiles.Contracts;


public sealed record ProfileAvatarUrlSetEvent(Guid Id, string? AvatarUrl) : IntegrationEvent(Id);