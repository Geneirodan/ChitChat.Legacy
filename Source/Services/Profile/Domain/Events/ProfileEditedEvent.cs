using Common.DDD;

namespace Domain.Events;

public sealed record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : DomainEvent(Id);