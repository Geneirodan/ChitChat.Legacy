using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Common.Http.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static System.Text.Encoding;

namespace Messages.Commands.IntegrationTests;

public class IntegrationTest
{
    protected IntegrationTest()
    {
        WebApi = new WebApiApplication();
        TestClient = WebApi.CreateClient();
    }
    
    protected virtual string Url => "api/v1";

    private WebApiApplication WebApi { get; }

    protected HttpClient TestClient { get; }

    protected Guid UserId { get; set; }

    protected void Authorize()
    {
        var jwtOptions = WebApi.Services.GetRequiredService<JwtOptions>();
        Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, UserId.ToString())];
        var symmetricSecurityKey = new SymmetricSecurityKey(UTF8.GetBytes(jwtOptions.Key));
        var jwt = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
    }
}