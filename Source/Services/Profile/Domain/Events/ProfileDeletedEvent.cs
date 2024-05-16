using Common.DomainDriven;

namespace Domain.Events;

public record ProfileDeletedEvent(Guid Id) : DomainEvent(Id);