using Common.Domain;

namespace Profiles.Domain.Aggregates;

public class Contact : Aggregate
{
    public Guid UserId { get; protected set; }
    public Guid ProfileId { get; protected set; }
    public string? FirstName { get; protected set; }
    public string? LastName { get; protected set; }

    public static (Contact profile, CreatedEvent @event) CreateInstance(Guid id, string? firstName,
        string? lastName, Guid userId, Guid profileId)
    {
        var contact = new Contact
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            UserId = userId,
            ProfileId = profileId
        };
        var @event = new CreatedEvent(
            contact.Id, 
            contact.FirstName, 
            contact.LastName, 
            contact.UserId,
            contact.ProfileId
        );
        contact.Enqueue(@event);
        return (contact, @event);
    }

    public EditedEvent Edit(string? firstName, string? lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        var @event = new EditedEvent(Id, FirstName, LastName);
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
    
    public record CreatedEvent(Guid Id, string? FirstName, string? LastName, Guid UserId, Guid ProfileId) : DomainEvent(Id);
    public record DeletedEvent(Guid Id) : DomainEvent(Id);
    public record EditedEvent(Guid Id, string? FirstName, string? LastName) : DomainEvent(Id);
}