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

internal static class AccountEndpoints
{
    internal static void MapAccount(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Account";
        var accountGroup = endpoints.MapGroup(groupName).WithTags(groupName).RequireAuthorization();
        accountGroup.MapPost(nameof(TwoFactor), TwoFactor);
        accountGroup.MapGet(nameof(Info), Info);
        accountGroup.MapPost(nameof(ChangeEmail), ChangeEmail);
        accountGroup.MapPost(nameof(ChangePassword), ChangePassword);
    }

    private static async Task<Results<Ok, ValidationProblem, NotFound>> ChangePassword(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] ChangePasswordRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        if (await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false) is not { } user)
            return TypedResults.NotFound();
        
        var (newPassword, oldPassword) = request;

        var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword).ConfigureAwait(false);

        return result.Succeeded ? TypedResults.Ok() : result.ToValidationProblem();
    }

    private static async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound, ForbidHttpResult>> ChangeEmail(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] ChangeEmailRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender
    )
    {
        if (await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false) is not { } user)
            return TypedResults.NotFound();

        var (newEmail, returnUrl) = request;

        if (await userManager.FindByEmailAsync(newEmail).ConfigureAwait(false) is not null)
            return TypedResults.Forbid();

        var email = await userManager.GetEmailAsync(user).ConfigureAwait(false);
        if (!string.Equals(email, newEmail, StringComparison.InvariantCultureIgnoreCase))
        {
            var code = await userManager.GenerateChangeEmailTokenAsync(user, newEmail).ConfigureAwait(false);
            await emailSender.SendEmailChangeConfirmationAsync(newEmail, code, returnUrl).ConfigureAwait(false);
        }

        var info = await userManager.CreateInfoResponseAsync(user).ConfigureAwait(false);
        return TypedResults.Ok(info);
    }

    private static async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>> Info(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] UserManager<User> userManager
    )
    {
        if (await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false) is not { } user)
            return TypedResults.NotFound();

        var info = await userManager.CreateInfoResponseAsync(user).ConfigureAwait(false);
        return TypedResults.Ok(info);
    }

    private static async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>> TwoFactor(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] TwoFactorRequest request,
        [FromServices] SignInManager<User> signInManager
    )
    {
        var userManager = signInManager.UserManager;
        if (await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false) is not { } user)
            return TypedResults.NotFound();

        var (enable, twoFactorCode, resetSharedKey, resetRecoveryCodes, forgetMachine) = request;
        
        if (enable == true)
        {
            var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
            if (!await userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, twoFactorCode!).ConfigureAwait(false))
                return TypedResults.ValidationProblem(new Dictionary<string, string[]> { { "InvalidTwoFactorCode", ["The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa."] } });
            
            await userManager.SetTwoFactorEnabledAsync(user, true).ConfigureAwait(false);
        }
        else if (enable == false || resetSharedKey)
            await userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);

        if (resetSharedKey) await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);

        string[]? recoveryCodes = null;
        if (resetRecoveryCodes ||
            (enable == true && await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false) == 0))
        {
            var recoveryCodesEnumerable =
                await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if (forgetMachine) await signInManager.ForgetTwoFactorClientAsync().ConfigureAwait(false);

        var key = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            key = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);

            if (string.IsNullOrEmpty(key))
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
        }

        var recoveryCodesLeft = recoveryCodes?.Length ??
                                await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false);
        var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
        var isMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user).ConfigureAwait(false);
        var twoFactorResponse = new TwoFactorResponse(key, recoveryCodesLeft, isTwoFactorEnabled, isMachineRemembered,
            recoveryCodes);
        return TypedResults.Ok(twoFactorResponse);
    }

    private static async Task<InfoResponse> CreateInfoResponseAsync(
        this UserManager<User> userManager,
        User user
    )
    {
        var id = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
        var email = await userManager.GetEmailAsync(user).ConfigureAwait(false) ??
                    throw new NotSupportedException("Users must have an email.");
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
        return new InfoResponse(id, email, isEmailConfirmed);
    }
}