using Common.DDD;

namespace Domain.Events;

public sealed record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : DomainEvent(Id);