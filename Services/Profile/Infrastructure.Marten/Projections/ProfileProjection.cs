using Marten.Events.Aggregation;
using Profiles.Application.ViewModels;
using Profiles.Domain;

namespace Profiles.Infrastructure.Marten.Projections;

public sealed class ProfileProjection : SingleStreamProjection<ProfileViewModel>
{
    public ProfileProjection()
    {
        CreateEvent<Profile.CreatedEvent>(Creator);
        ProjectEvent<Profile.EditedEvent>(EditEvent);
        DeleteEvent<Profile.DeletedEvent>();
    }

    private static ProfileViewModel Creator(Profile.CreatedEvent e) => 
        new(e.Id, e.FirstName, e.LastName, string.Empty);
    
    private static ProfileViewModel EditEvent(ProfileViewModel model, Profile.EditedEvent e) => 
        model with { FirstName = e.FirstName, LastName = e.LastName, Bio = e.Bio };

}