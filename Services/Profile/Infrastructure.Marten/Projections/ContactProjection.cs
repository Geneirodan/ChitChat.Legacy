using Marten.Events.Aggregation;
using Profiles.Application.ViewModels;
using Profiles.Domain;

namespace Profiles.Infrastructure.Marten.Projections;

public sealed class ContactProjection : SingleStreamProjection<ContactViewModel>
{
    public ContactProjection()
    {
        CreateEvent<Contact.CreatedEvent>(Creator);
        ProjectEvent<Contact.EditedEvent>(EditEvent);
        DeleteEvent<Contact.DeletedEvent>();
    }

    private static ContactViewModel Creator(Contact.CreatedEvent e) => 
        new(e.Id, e.FirstName, e.LastName, e.UserId, e.ProfileId);
    
    private static ContactViewModel EditEvent(ContactViewModel model, Contact.EditedEvent e) => 
        model with { FirstName = e.FirstName, LastName = e.LastName};

}