namespace Identity.Endpoints;

internal static class IdentityEndpoints
{
    public static void MapIdentity(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuth();
        endpoints.MapEmail();
        endpoints.MapPassword();
        endpoints.MapAccount();
    }
}