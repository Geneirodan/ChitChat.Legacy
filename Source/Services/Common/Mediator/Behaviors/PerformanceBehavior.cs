using System.Diagnostics;
using Common.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Mediator.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(ILogger<TRequest> logger, IUser user) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    
    private const long Threshold = 500;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next().ConfigureAwait(false);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= Threshold)
            return response;

        logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} ms) {UserId} {@Request}", 
            typeof(TRequest).Name, elapsedMilliseconds, user.Id, request);

        return response;
    }
}
