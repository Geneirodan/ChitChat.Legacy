using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Common.Behaviors;

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

        var result = validationResults.Select(ToFluentResult).Merge();

        return result.IsSuccess ? await next().ConfigureAwait(false) : new TResponse().WithErrors(result.Errors);
    }

    private static Result ToFluentResult(ValidationResult result)
    {
        var errors = result.IsValid
            ? []
            : result.Errors.GroupBy(x => x.PropertyName,
                (propertyName, validationFailures) =>
                {
                    var messages = validationFailures.Select(x => new Error(x.ErrorMessage));
                    var error = new Error(propertyName);
                    error.Reasons.AddRange(messages);
                    return error;
                });
        return new Result().WithErrors(errors);
    }
}