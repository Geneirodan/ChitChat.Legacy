using Common;

namespace Messages.Contracts.IntegrationEvents;

[ExcludeFromTopology]
public record MessageEvent(Guid Id, Guid SenderId, Guid ReceiverId) : IntegrationEvent(Id);