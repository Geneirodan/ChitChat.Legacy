using System.Reflection;
using Common.MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Messages.Queries.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var options = new MediatRPipelineOptions { UseValidation = false };
        return services.AddMediatRPipeline(Assembly.GetExecutingAssembly(), options);
    }
}