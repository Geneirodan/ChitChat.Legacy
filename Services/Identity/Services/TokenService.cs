using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common.Http.Jwt;
using Identity.Options;
using Identity.Persistence;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.StringComparison;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;

namespace Identity.Services;

internal sealed class TokenService(
    IOptions<JwtOptions> jwtOptions,
    IOptions<ExpirationOptions> expirationOptions,
    ApplicationContext context,
    UserManager<User> userManager,
    TimeProvider timeProvider
) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly ExpirationOptions _expirationOptions = expirationOptions.Value;
    private readonly SymmetricSecurityKey _symmetricSecurityKey = new(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));

    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> GenerateTokens(User user)
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        ];
        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expirationOptions.AccessToken)),
            signingCredentials: new SigningCredentials(_symmetricSecurityKey, HmacSha256));

        var refreshToken = GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Value = refreshToken,
            ExpiresAt = timeProvider.GetUtcNow().AddDays(_expirationOptions.RefreshToken)
        });
        if (await context.SaveChangesAsync() == 0)
            return TypedResults.Unauthorized();

        var accessTokenResponse = new AccessTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiresIn = _expirationOptions.AccessToken,
            RefreshToken = refreshToken
        };
        return TypedResults.Ok(accessTokenResponse);
    }

    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> ValidateExpiredTokens(
        string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal is null)
            return TypedResults.Unauthorized();

        var user = await userManager.GetUserAsync(principal);
        if (user is null)
            return TypedResults.Unauthorized();

        var refreshTokenEntity =
            await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Value == refreshToken);

        if (refreshTokenEntity is null || refreshTokenEntity.ExpiresAt <= timeProvider.GetUtcNow())
            return TypedResults.Unauthorized();

        context.RefreshTokens.Remove(refreshTokenEntity);
        await context.SaveChangesAsync();

        return await GenerateTokens(user);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _symmetricSecurityKey,
            ValidateLifetime = false
        };
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenValidationParameters, out var securityToken);

        return securityToken is JwtSecurityToken jwtSecurityToken
               && jwtSecurityToken.Header.Alg.Equals(HmacSha256, InvariantCultureIgnoreCase)
            ? principal
            : null;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}