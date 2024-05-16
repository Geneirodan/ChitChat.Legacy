using Common.DomainDriven;
using Domain.Events;

namespace Domain
{
    public class Profile : Aggregate
    {
        public string FirstName { get; protected set; } = null!;
        public string LastName { get; protected set; } = string.Empty;
        public string Bio { get; protected set; } = string.Empty;
        public string? AvatarUrl { get; protected set; }

        public static (Profile profile, ProfileCreatedEvent @event) CreateInstance(Guid id, string firstName, string lastName)
        {
            var profile = new Profile
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };
            var @event = new ProfileCreatedEvent(profile.Id, profile.FirstName, profile.LastName);
            profile.Enqueue(@event);
            return (profile, @event);
        }

        public ProfileEditedEvent Edit(string firstName, string lastName, string bio)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            var @event = new ProfileEditedEvent(Id, FirstName, LastName, Bio);
            Enqueue(@event);
            return @event;
        }

        public ProfileSetAvatarEvent SetAvatar(string? avatarUrl)
        {
            AvatarUrl = avatarUrl;
            var @event = new ProfileSetAvatarEvent(Id, AvatarUrl);
            Enqueue(@event);
            return @event;
        }

        public ProfileDeletedEvent Delete()
        {
            IsDeleted = true;
            var @event = new ProfileDeletedEvent(Id);
            Enqueue(@event);
            return @event;
        }
    }
}