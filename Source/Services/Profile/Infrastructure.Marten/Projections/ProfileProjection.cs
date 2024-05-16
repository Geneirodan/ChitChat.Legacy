using Application;
using Domain.Events;
using Marten.Events.Aggregation;

namespace Infrastructure.Marten.Projections;

public sealed class ProfileProjection : SingleStreamProjection<ProfileViewModel>
{
    public ProfileProjection()
    {
        CreateEvent<ProfileCreatedEvent>(Creator);
        ProjectEvent<ProfileEditedEvent>(EditEvent);
        DeleteEvent<ProfileDeletedEvent>();
    }

    private static ProfileViewModel Creator(ProfileCreatedEvent e) => 
        new(e.Id, e.FirstName, e.LastName, string.Empty);
    
    private static ProfileViewModel EditEvent(ProfileViewModel model, ProfileEditedEvent e) => 
        model with { FirstName = e.FirstName, LastName = e.LastName, Bio = e.Bio };

}