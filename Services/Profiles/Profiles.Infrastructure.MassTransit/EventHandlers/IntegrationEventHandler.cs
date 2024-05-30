using Common;
using Common.Domain;
using Mapster;
using MediatR;

namespace Profiles.Infrastructure.MassTransit.EventHandlers;

public abstract class IntegrationEventHandler<TDomainEvent, TIntegrationEvent>(IPublishEndpoint endpoint)
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Handle(TDomainEvent request, CancellationToken cancellationToken)
    {
        var @event = request.Adapt<TIntegrationEvent>();
        await endpoint.Publish(@event, cancellationToken).ConfigureAwait(false);
    }
}