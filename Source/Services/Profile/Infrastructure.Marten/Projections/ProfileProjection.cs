using Marten.Events.Aggregation;
using Profile.Application;
using Profile.Domain.Events;

namespace Profile.Infrastructure.Marten.Projections;

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