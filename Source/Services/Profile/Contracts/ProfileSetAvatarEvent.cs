using Common;

namespace Contracts;

public record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : IntegrationEvent(Id);