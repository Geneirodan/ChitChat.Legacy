using Identity.Extensions;
using Identity.Persistence;
using Identity.Requests;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.Endpoints;

internal static class AuthEndpoints
{
    internal static void MapAuth(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Auth";
        var routeGroup = endpoints.MapGroup(groupName).WithTags(groupName);
        routeGroup.MapPost(nameof(Register), Register);
        routeGroup.MapPost(nameof(Login), Login);
        routeGroup.MapPost(nameof(Refresh), Refresh);
    }

    private static async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> Refresh(
        [FromBody] RefreshRequest request,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        [FromServices] TimeProvider timeProvider,
        [FromServices] ITokenService tokenService
    )
    {
        var (accessToken, refreshToken) = request;
        return await tokenService.ValidateExpiredTokens(accessToken, refreshToken).ConfigureAwait(false);
    }

    private static async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> Login(
        [FromBody] LoginRequest request,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] ITokenService tokenService
    )
    {
        var (username, password, isPersistent, twoFactorCode, twoFactorRecoveryCode) = request;
        var userManager = signInManager.UserManager;

        var user = await userManager.FindByEmailAsync(username).ConfigureAwait(false) ?? await userManager.FindByNameAsync(username).ConfigureAwait(false);
        if (user is null)
            return TypedResults.Unauthorized();

        var result = await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: true).ConfigureAwait(false);

        if (!result.RequiresTwoFactor)
            return result.Succeeded
                ? await tokenService.GenerateTokens(user).ConfigureAwait(false)
                : TypedResults.Unauthorized();
        if (!string.IsNullOrEmpty(twoFactorCode))
            result = await signInManager.TwoFactorAuthenticatorSignInAsync(twoFactorCode, isPersistent,
                isPersistent).ConfigureAwait(false);
        else if (!string.IsNullOrEmpty(twoFactorRecoveryCode))
            result = await signInManager.TwoFactorRecoveryCodeSignInAsync(twoFactorRecoveryCode).ConfigureAwait(false);

        return result.Succeeded
            ? await tokenService.GenerateTokens(user).ConfigureAwait(false)
            : TypedResults.Unauthorized();
    }

    private static async Task<Results<Created, ValidationProblem>> Register(
        [FromBody] RegisterRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender)
    {
        var (username, email, password, returnUrl) = request;

        var user = new User { Email = email, UserName = username };
        var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);

        if (!result.Succeeded)
            return result.ToValidationProblem();

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
        await emailSender.SendRegisterConfirmationAsync(email, code, returnUrl).ConfigureAwait(false);

        return TypedResults.Created();
    }
}