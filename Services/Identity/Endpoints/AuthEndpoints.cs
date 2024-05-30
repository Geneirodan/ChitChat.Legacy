using Identity.Extensions;
using Identity.Persistence;
using Identity.Requests;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.String;

namespace Identity.Endpoints;

using RegisterResponse = Results<Created, ValidationProblem>;
using LoginResponse = Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>;
using RefreshResponse = Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>;

internal static class AuthEndpoints
{
    private const string GroupName = "auth";

    internal static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var routeGroup = endpoints.MapGroup(GroupName).WithTags(GroupName);
        routeGroup.MapPost("/register", RegisterAsync);
        routeGroup.MapPost("/login", LoginAsync);
        routeGroup.MapPost("/refresh", RefreshAsync);
    }

    private static async Task<RefreshResponse> RefreshAsync(
        [FromBody] RefreshRequest request,
        [FromServices] ITokenService tokenService
    )
    {
        var (accessToken, refreshToken) = request;
        return await tokenService.ValidateExpiredTokens(accessToken, refreshToken);
    }

    private static async Task<LoginResponse> LoginAsync(
        [FromQuery] bool rememberClient,
        [FromQuery] bool isPersistent,
        [FromBody] LoginRequest request,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] ITokenService tokenService)
    {
        var (username, password, twoFactorCode, twoFactorRecoveryCode) = request;
        var userManager = signInManager.UserManager;

        var user = await userManager.FindByEmailAsync(username) ?? await userManager.FindByNameAsync(username);
        if (user is null)
            return TypedResults.Unauthorized();

        var result = await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: true);

        if (!result.RequiresTwoFactor)
            return result.Succeeded
                ? await tokenService.GenerateTokens(user)
                : TypedResults.Unauthorized();

        if (!IsNullOrEmpty(twoFactorCode))
            result = await signInManager.TwoFactorAuthenticatorSignInAsync(twoFactorCode, isPersistent,
                rememberClient);
        else if (!IsNullOrEmpty(twoFactorRecoveryCode))
            result = await signInManager.TwoFactorRecoveryCodeSignInAsync(twoFactorRecoveryCode);

        return result.Succeeded
            ? await tokenService.GenerateTokens(user)
            : TypedResults.Unauthorized();
    }

    private static async Task<RegisterResponse> RegisterAsync(
        [FromBody] RegisterRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] IEmailSender emailSender)
    {
        var (username, email, password, returnUrl) = request;

        var user = new User { Email = email, UserName = username };
        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return result.ToValidationProblem();

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await emailSender.SendRegisterConfirmationAsync(email, code, returnUrl);

        return TypedResults.Created();
    }
}