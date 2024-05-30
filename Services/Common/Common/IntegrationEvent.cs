using MediatR;

namespace Common;

public abstract record IntegrationEvent(Guid Id): INotification;