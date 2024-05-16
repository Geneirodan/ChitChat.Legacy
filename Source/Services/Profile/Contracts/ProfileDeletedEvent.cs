using Common.Other;

namespace Contracts;


public sealed record ProfileDeletedEvent(Guid Id) : IntegrationEvent(Id);