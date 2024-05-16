using Common.Other;

namespace Profile.Contracts;


public sealed record ProfileDeletedEvent(Guid Id) : IntegrationEvent(Id);