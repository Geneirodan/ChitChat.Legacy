using System.Security.Claims;
using Identity.Extensions;
using Identity.Persistence;
using Identity.Requests;
using Identity.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Endpoints;

using Enable2FAResponse = Results<Ok<TwoFactorResponse>, ValidationProblem, UnauthorizedHttpResult>;

internal static class TwoFactorEndpoints
{
    internal static void MapTwoFactorEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "2fa";
        var accountGroup = endpoints.MapGroup(groupName).WithTags(groupName).RequireAuthorization();
        accountGroup.MapPost(string.Empty, EnableTwoFactor);
        accountGroup.MapDelete(string.Empty, DisableTwoFactor);
        accountGroup.MapPost("reset/recoveryCodes", ResetRecoveryCodes);
        accountGroup.MapPost("reset/sharedKey", ResetSharedKey);
        accountGroup.MapPost("forget2fa", ForgetTwoFactor);
    }

    private static async Task<Enable2FAResponse> EnableTwoFactor(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] TwoFactorRequest request,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return TypedResults.Unauthorized();

        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        if (!await userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, request.TwoFactorCode))
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                {
                    nameof(request.TwoFactorCode),
                    ["The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa."]
                }
            });

        var result = await userManager.SetTwoFactorEnabledAsync(user, true);
        if (!result.Succeeded)
            return result.ToValidationProblem();

        var recoveryCodesEnumerable =
            await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        var recoveryCodes = recoveryCodesEnumerable?.ToArray();

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
        }

        var response = new TwoFactorResponse(key, recoveryCodes);
        return TypedResults.Ok(response);
    }
    
    private static async Task<Results<Ok, ValidationProblem, UnauthorizedHttpResult>> DisableTwoFactor(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return TypedResults.Unauthorized();

        var result = await userManager.SetTwoFactorEnabledAsync(user, false);
        return result.Succeeded ? TypedResults.Ok() : result.ToValidationProblem();
    }

    private static async Task<Results<Ok<IEnumerable<string>>, ValidationProblem, UnauthorizedHttpResult>> ResetRecoveryCodes(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return TypedResults.Unauthorized();

        var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        return TypedResults.Ok(recoveryCodes);
    }

    private static async Task<Results<Ok<string>, ValidationProblem, UnauthorizedHttpResult>> ResetSharedKey(
        ClaimsPrincipal claimsPrincipal,
        [FromServices] UserManager<User> userManager
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return TypedResults.Unauthorized();
        
        if (await userManager.SetTwoFactorEnabledAsync(user, false) is { Succeeded: false } setResult)
            return setResult.ToValidationProblem();
        
        if (await userManager.ResetAuthenticatorKeyAsync(user) is { Succeeded: false } resetResult)
            return resetResult.ToValidationProblem();

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        return TypedResults.Ok(key);
    }
    
    private static async Task ForgetTwoFactor([FromServices] SignInManager<User> signInManager) =>
        await signInManager.ForgetTwoFactorClientAsync();

}