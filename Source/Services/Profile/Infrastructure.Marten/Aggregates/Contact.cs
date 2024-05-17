namespace Profiles.Infrastructure.Marten.Aggregates;

public sealed class Contact : Domain.Contact
{
    public void Apply(CreatedEvent @event) => (Id, FirstName, LastName, UserId, ProfileId) = @event;
    public void Apply(DeletedEvent _) => IsDeleted = true;
    public void Apply(EditedEvent @event) => (_, FirstName, LastName) = @event;
}