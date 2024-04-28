using FluentValidation;
using MediatR.Pipeline;
using System.Reflection;
using Common.Behaviors;
using Common.Options;
using Common.Options.Configurations;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Common.Roles;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration section)
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
    
    public static IServiceCollection AddMediatR(this IServiceCollection services, params Assembly[] assemblies) =>
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(LoggingBehavior<>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });
}
