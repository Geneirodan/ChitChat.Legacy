using System.Text;
using Identity.Extensions;
using Identity.Persistence;
using Identity.Requests;
using Identity.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Identity.Endpoints;

internal static class PasswordEndpoints
{
    public static void MapPassword(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Password";
        var routeGroup = endpoints.MapGroup(groupName).WithTags(groupName);
        routeGroup.MapPost(nameof(Forgot), Forgot);
        routeGroup.MapPost(nameof(Reset), Reset);
    }

    private static async Task<Results<Ok, ValidationProblem>> Reset(
        [FromBody] ResetPasswordRequest resetRequest,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.FindByEmailAsync(resetRequest.Email).ConfigureAwait(false);

        IdentityResult result;
        if (user is not null && await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
        {
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
                result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword).ConfigureAwait(false);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }
        }
        else
            result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());

        return result.Succeeded ? TypedResults.Ok() : result.ToValidationProblem();
    }

    private static async Task<Results<Ok, ValidationProblem>> Forgot(
        [FromBody] ForgotPasswordRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        var (email, resetUrl) = request;
        var user = await userManager.FindByEmailAsync(email).ConfigureAwait(false);

        if (user is null || !await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false)) 
            return TypedResults.Ok();

        var code = await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        await emailSender.SendPasswordResetCodeAsync(email, code, resetUrl).ConfigureAwait(false);
        return TypedResults.Ok();
    }
}