using Common.Other;

namespace Contracts;


public sealed record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : IntegrationEvent(Id);