using Identity.Persistence;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Services;

internal interface ITokenService
{
    Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> GenerateTokens(User user);
    Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult>> ValidateExpiredTokens(string accessToken, string refreshToken);
}