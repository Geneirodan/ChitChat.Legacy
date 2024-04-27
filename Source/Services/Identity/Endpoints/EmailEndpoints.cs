using System.Text;
using Identity.Persistence;
using Identity.Requests;
using Identity.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Identity.Endpoints;

internal static class EmailEndpoints
{
    public static void MapEmail(this IEndpointRouteBuilder endpoints)
    {
        var routeGroup = endpoints.MapGroup("email");
        routeGroup.MapGet("confirm", ConfirmEmail);
        routeGroup.MapPost("resend", ResendConfirmationEmail);
    }

    private static async Task<Ok> ResendConfirmationEmail(
        [FromBody] ResendConfirmationEmailRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        var (email, confirmUrl) = request;
        if (await userManager.FindByEmailAsync(email).ConfigureAwait(false) is not { } user)
            return TypedResults.Ok();

        var code = await userManager.GenerateChangeEmailTokenAsync(user, email).ConfigureAwait(false);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        await emailSender.SendRegisterConfirmationAsync(email, code, confirmUrl).ConfigureAwait(false);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, BadRequest>> ConfirmEmail(
        [FromQuery] string email,
        [FromQuery] string code,
        [FromQuery] string? changedEmail,
        [FromServices] UserManager<User> userManager
    )
    {
        if (await userManager.FindByEmailAsync(email).ConfigureAwait(false) is not { } user)
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
            ? await userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false)
            : await userManager.ChangeEmailAsync(user, changedEmail, code).ConfigureAwait(false);

        return result.Succeeded ? TypedResults.Ok() : TypedResults.BadRequest();
    }
}