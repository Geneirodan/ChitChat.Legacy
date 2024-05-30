using System.Diagnostics;
using Common.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.MediatR.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<TRequest> logger,
    IUser user,
    IConfiguration configuration)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly long _threshold =
        long.TryParse(configuration.GetSection("Performance")["Threshold"], out var longValue) ? longValue : 1000;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        var response = await next().ConfigureAwait(false);

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds <= _threshold)
            return response;

        logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} ms) {UserId} {@Request}",
            typeof(TRequest).Name, elapsedMilliseconds, user.Id, request);

        return response;
    }
}