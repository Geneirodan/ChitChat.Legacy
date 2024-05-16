using Common.DomainDriven;

namespace Domain.Events;

public record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : DomainEvent(Id);