using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Identity.Endpoints;

internal static class IdentityEndpoints
{
    internal static void MapIdentity(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(string.Empty).AddFluentValidationAutoValidation();
        group.MapAuth();
        group.MapEmail();
        group.MapPassword();
        group.MapAccount();
    }
}