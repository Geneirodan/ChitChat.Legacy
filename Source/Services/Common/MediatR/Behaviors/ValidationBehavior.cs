using Common.Extensions;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Common.MediatR.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase<TResponse>, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next().ConfigureAwait(false);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)))
            .ConfigureAwait(false);

        var result = validationResults.Select(ValidationResultExtensions.ToFluentResult).Merge();

        return result.IsSuccess ? await next().ConfigureAwait(false) : new TResponse().WithErrors(result.Errors);
    }
}