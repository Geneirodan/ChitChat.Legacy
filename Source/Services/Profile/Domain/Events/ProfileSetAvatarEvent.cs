using Common.DDD;

namespace Profile.Domain.Events;

public sealed record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : DomainEvent(Id);