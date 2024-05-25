using System.Text;
using Identity.Persistence;
using Identity.Requests;
using Identity.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Identity.Endpoints;

using ConfirmResponse = Results<Ok, BadRequest>;
using ResendResponse = Results<Ok, Conflict>;

internal static class EmailEndpoints
{
    private const string GroupName = "email";
    internal static void MapEmailEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var routeGroup = endpoints.MapGroup(GroupName).WithTags(GroupName);
        routeGroup.MapGet("/confirm", ConfirmEmailAsync);
        routeGroup.MapPost("/resend", ResendConfirmationEmailAsync);
    }

    private static async Task<ResendResponse> ResendConfirmationEmailAsync(
        [FromBody] ResendEmailRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        var (email, confirmUrl) = request;
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || user.EmailConfirmed)
            return TypedResults.Ok();
        if (user.EmailConfirmed)
            return TypedResults.Conflict();

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        await emailSender.SendRegisterConfirmationAsync(email, code, confirmUrl);
        return TypedResults.Ok();
    }

    private static async Task<ConfirmResponse> ConfirmEmailAsync(
        [FromQuery] string email,
        [FromQuery] string code,
        [FromQuery] string? changedEmail,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return TypedResults.BadRequest();

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return TypedResults.BadRequest();
        }

        var result = string.IsNullOrEmpty(changedEmail)
            ? await userManager.ConfirmEmailAsync(user, code)
            : await userManager.ChangeEmailAsync(user, changedEmail, code);

        return result.Succeeded ? TypedResults.Ok() : TypedResults.BadRequest();
    }
}