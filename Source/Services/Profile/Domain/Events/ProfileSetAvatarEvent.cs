using Common.DomainDriven;

namespace Domain.Events;

public record ProfileSetAvatarEvent(Guid Id, string? AvatarUrl) : DomainEvent(Id);