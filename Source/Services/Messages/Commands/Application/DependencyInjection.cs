using FluentValidation;
using System.Reflection;
using Common.MediatR;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true)
            .AddMediatRPipeline(Assembly.GetExecutingAssembly());
}