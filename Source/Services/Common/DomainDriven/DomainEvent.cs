using MediatR;

namespace Common.DomainDriven;

public record DomainEvent(Guid Id) : INotification;