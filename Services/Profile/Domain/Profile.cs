using Common.DDD;

namespace Profiles.Domain;

public class Profile : Aggregate
{
    public string FirstName { get; protected set; } = null!;
    public string LastName { get; protected set; } = string.Empty;
    public string Bio { get; protected set; } = string.Empty;
    public string? AvatarUrl { get; protected set; }

    public static (Profile profile, CreatedEvent @event) CreateInstance(Guid id, string firstName, string lastName)
    {
        var profile = new Profile
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName
        };
        var @event = new CreatedEvent(profile.Id, profile.FirstName, profile.LastName);
        profile.Enqueue(@event);
        return (profile, @event);
    }

    public EditedEvent Edit(string firstName, string lastName, string bio)
    {
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        var @event = new EditedEvent(Id, FirstName, LastName, Bio);
        Enqueue(@event);
        return @event;
    }

    public AvatarUrlSetEvent SetAvatar(string? avatarUrl)
    {
        AvatarUrl = avatarUrl;
        var @event = new AvatarUrlSetEvent(Id, AvatarUrl);
        Enqueue(@event);
        return @event;
    }

    public DeletedEvent Delete()
    {
        IsDeleted = true;
        var @event = new DeletedEvent(Id);
        Enqueue(@event);
        return @event;
    }
    
    public sealed record CreatedEvent(Guid Id, string FirstName, string LastName) : DomainEvent(Id);
    public sealed record UserNameChangedEvent(Guid Id, string FirstName) : DomainEvent(Id);
    public sealed record EditedEvent(Guid Id, string FirstName, string LastName, string Bio) : DomainEvent(Id);
    public sealed record AvatarUrlSetEvent(Guid Id, string? AvatarUrl) : DomainEvent(Id);
    public sealed record DeletedEvent(Guid Id) : DomainEvent(Id);
}