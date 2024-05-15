using System.Reflection;
using Common.MediatR;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var options = new MediatRPipelineOptions { UseValidation = false };
        return services.AddMediatRPipeline(Assembly.GetExecutingAssembly(), options);
    }
}