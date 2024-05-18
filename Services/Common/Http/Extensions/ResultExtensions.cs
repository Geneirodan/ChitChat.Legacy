using Common.Results;
using FluentResults;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Http.TypedResults;

namespace Common.Http.Extensions;

public static class ResultExtensions
{
    public static TResult WithReason<TResult>(this ResultBase<TResult> result, string reason) 
        where TResult : ResultBase<TResult> => result.WithReason(new Error(reason)); 
    
    private static IResult ResultToError(this ResultBase result)
    {
        if (result.IsNotFound())
            return NotFound();

        if (result.IsForbidden())
            return Forbid();

        if (result.IsUnauthorized())
            return Unauthorized();

        Dictionary<string, List<string>> errors = [];
        
        foreach (var error in result.Errors)
            if (error.Reasons.Count > 0)
            {
                errors.TryAdd(error.Message, []);
                error.Reasons.ForEach(x => errors[error.Message].Add(x.Message));
            }
            else
            {
                errors.TryAdd(string.Empty, []);
                errors[string.Empty].Add(error.Message);
            }

        return ValidationProblem(errors.ToDictionary(x => x.Key, x => x.Value.ToArray()));
    }

    public static IResult ResultToResponse(this Result result, int statusCode = StatusCodes.Status200OK) => 
        result.IsFailed ? result.ResultToError() : StatusCode(statusCode);

    public static IResult ResultToResponse<T>(this Result<T> result, int statusCode = StatusCodes.Status200OK) =>
        result.IsFailed ? result.ResultToError() : Json(result.ValueOrDefault, statusCode: statusCode);
}