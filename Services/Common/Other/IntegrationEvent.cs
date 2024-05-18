using MediatR;

namespace Common.Other;

public abstract record IntegrationEvent(Guid Id): INotification;