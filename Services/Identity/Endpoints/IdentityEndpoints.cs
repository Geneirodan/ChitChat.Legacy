using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Identity.Endpoints;

internal static class IdentityEndpoints
{
    internal static void MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(string.Empty).AddFluentValidationAutoValidation();
        group.MapAuthEndpoints();
        group.MapEmailEndpoints();
        group.MapPasswordEndpoints();
        group.MapAccountEndpoints();
        group.MapTwoFactorEndpoints();
    }
}