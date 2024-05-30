using Common;

namespace Profiles.Contracts;


public sealed record ProfileDeletedEvent(Guid Id) : IntegrationEvent(Id);