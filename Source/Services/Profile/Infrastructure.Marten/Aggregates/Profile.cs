using Domain.Events;

namespace Infrastructure.Marten.Aggregates;


public sealed class Profile : Domain.Profile
{
    public void Apply(ProfileCreatedEvent @event) => (Id, FirstName, LastName) = @event;
    public void Apply(ProfileDeletedEvent _) => IsDeleted = true;
    public void Apply(ProfileEditedEvent @event) => (_, FirstName, LastName, Bio) = @event;
    public void Apply(ProfileSetAvatarEvent @event) => AvatarUrl = @event.AvatarUrl;
}