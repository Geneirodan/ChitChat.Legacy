using Common.DDD;

namespace Profile.Domain.Events;

public sealed record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : DomainEvent(Id);