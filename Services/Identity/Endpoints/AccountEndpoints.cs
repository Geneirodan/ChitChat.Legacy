using System.Security.Claims;
using Identity.Extensions;
using Identity.Persistence;
using Identity.Requests;
using Identity.Responses;
using Identity.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Endpoints;

using InfoResponse = Results<Ok<UserViewModel>, ValidationProblem, UnauthorizedHttpResult>;
using ChangeEmailResponse = Results<Ok, ValidationProblem, UnauthorizedHttpResult, ForbidHttpResult, Conflict>;
using ChangePasswordResponse = Results<Ok, ValidationProblem, UnauthorizedHttpResult>;

internal static class AccountEndpoints
{
    private const string GroupName = "account";

    internal static void MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var accountGroup = endpoints.MapGroup(GroupName).WithTags(GroupName).RequireAuthorization();
        accountGroup.MapGet("/", InfoAsync);
        accountGroup.MapPatch("/email", ChangeEmailAsync);
        accountGroup.MapPatch("/password", ChangePasswordAsync);
    }

    private static async Task<ChangePasswordResponse> ChangePasswordAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] ChangePasswordRequest request,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);

        if (user is null)
            return TypedResults.Unauthorized();

        var (newPassword, oldPassword) = request;

        var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return result.Succeeded ? TypedResults.Ok() : result.ToValidationProblem();
    }

    private static async Task<ChangeEmailResponse> ChangeEmailAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] ChangeEmailRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);

        if (user is null)
            return TypedResults.Unauthorized();

        var (newEmail, returnUrl) = request;

        if (await userManager.FindByEmailAsync(newEmail) is not null)
            return TypedResults.Forbid();

        var email = await userManager.GetEmailAsync(user);
        if (newEmail.Equals(email, StringComparison.InvariantCultureIgnoreCase)) 
            return TypedResults.Conflict();
        
        var code = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        await emailSender.SendEmailChangeConfirmationAsync(newEmail, code, returnUrl);
        return TypedResults.Ok();
    }

    private static async Task<InfoResponse> InfoAsync(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return TypedResults.Unauthorized();

        var info = new UserViewModel(user);
        return TypedResults.Ok(info);
    }
}