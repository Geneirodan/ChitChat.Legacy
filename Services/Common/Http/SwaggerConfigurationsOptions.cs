using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Http;

public sealed class SwaggerConfigurationsOptions : IConfigureOptions<SwaggerGenOptions>
{

    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                Description = "Authorization using Bearer scheme 'Bearer <token>'",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = JwtConstants.TokenType
            });
        var openApiSecurityScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
        };
        options.AddSecurityRequirement(new OpenApiSecurityRequirement { { openApiSecurityScheme, [] } });
    }
}