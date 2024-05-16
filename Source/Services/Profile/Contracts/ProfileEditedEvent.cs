using Common;

namespace Contracts;

public record ProfileEditedEvent(Guid Id, string FirstName, string LastName, string Bio) : IntegrationEvent(Id);