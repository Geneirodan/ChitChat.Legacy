using Common.Other;

namespace Profiles.Contracts;


public sealed record ProfileDeletedEvent(Guid Id) : IntegrationEvent(Id);