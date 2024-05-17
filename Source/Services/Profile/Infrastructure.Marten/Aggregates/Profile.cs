namespace Profiles.Infrastructure.Marten.Aggregates;

public sealed class Profile : Domain.Profile
{
    public void Apply(CreatedEvent @event) => (Id, FirstName, LastName) = @event;
    public void Apply(DeletedEvent _) => IsDeleted = true;
    public void Apply(EditedEvent @event) => (_, FirstName, LastName, Bio) = @event;
    public void Apply(AvatarUrlSetEvent @event) => AvatarUrl = @event.AvatarUrl;
}