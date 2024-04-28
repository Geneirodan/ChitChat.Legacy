using FluentResults;
using FluentValidation.Results;

// ReSharper disable UnusedMethodReturnValue.Global

namespace Common.Extensions;

internal static class ValidationResultExtensions
{
    public static Result ToFluentResult(this ValidationResult result)
    {
        if (result.IsValid) return Result.Ok();
        var errors = result.Errors.GroupBy(x => x.PropertyName, (s, failures) =>
        {
            var e = new Error(s);
            e.Reasons.AddRange(failures.Select(x => new Error(x.ErrorMessage)));
            return e;
        });
        return Result.Fail(errors);

    }
}