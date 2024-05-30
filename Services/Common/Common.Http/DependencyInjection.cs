using Common.Abstractions;
using Common.Http.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Common.Roles;

namespace Common.Http;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration section)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Admin), builder => builder.RequireRole(nameof(Admin)))
            .AddPolicy(nameof(User), builder => builder.RequireRole(nameof(User)));

        return services.Configure<JwtOptions>(section)
            .AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerConfigurationOptions>();
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationsOptions>();

    public static IServiceCollection AddHttpUser(this IServiceCollection services) =>
        services
            .AddHttpContextAccessor()
            .AddScoped<IUser, HttpUser>();
}