using Common;

namespace Contracts;

public record ProfileDeletedEvent(Guid Id) : IntegrationEvent(Id);