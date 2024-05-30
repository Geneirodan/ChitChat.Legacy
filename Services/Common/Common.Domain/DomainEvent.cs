using MediatR;

namespace Common.Domain;

public abstract record DomainEvent(Guid Id) : INotification;