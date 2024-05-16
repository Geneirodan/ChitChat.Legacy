using Common.Other;

namespace Profile.Contracts;


public sealed record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : IntegrationEvent(Id);