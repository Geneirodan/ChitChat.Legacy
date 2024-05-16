using MassTransit;
using MediatR;

namespace Messages.Commands.Infrastructure.MassTransit.EventHandlers;

public abstract class IntegrationEventHandler<TEvent>(IPublishEndpoint endpoint)
    : INotificationHandler<TEvent>
    where TEvent : INotification
{
    public async Task Handle(TEvent request, CancellationToken cancellationToken) => 
        await endpoint.Publish(request, cancellationToken).ConfigureAwait(false);
}