using MediatR;

namespace Common.DDD;

public abstract record DomainEvent(Guid Id) : INotification;