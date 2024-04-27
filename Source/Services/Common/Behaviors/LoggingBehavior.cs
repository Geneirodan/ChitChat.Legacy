using Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Common.Behaviors;

public sealed class LoggingBehavior<TRequest>(ILogger<TRequest> logger, IUser user) 
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {Name} {UserId} {@Request}", typeof(TRequest).Name, user.Id, request);
        return Task.CompletedTask;
    }
}