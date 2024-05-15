using FluentResults;
using Microsoft.AspNetCore.Http;
using static System.Net.HttpStatusCode;

namespace Common.Extensions;

public static class ResultExtensions
{
    public static TResult WithReason<TResult>(this ResultBase<TResult> result, string reason) 
        where TResult : ResultBase<TResult> => result.WithReason(new Error(reason)); 
    
    private static IResult ResultToError(this ResultBase result)
    {
        if (result.HasError(x => x.Message == nameof(NotFound)))
            return Results.NotFound();

        if (result.HasError(x => x.Message == nameof(Forbidden)))
            return Results.Forbid();

        if (result.HasError(x => x.Message == nameof(Unauthorized)))
            return Results.Unauthorized();

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

        return Results.ValidationProblem(errors.ToDictionary(x => x.Key, x => x.Value.ToArray()));
    }

    public static IResult ResultToResponse(this Result result, int statusCode = StatusCodes.Status200OK) =>
        result.IsFailed ? result.ResultToError() : Results.StatusCode(statusCode);

    public static IResult ResultToResponse<T>(this Result<T> result, int statusCode = StatusCodes.Status200OK) =>
        result.IsFailed ? result.ResultToError() : Results.Json(result.ValueOrDefault, statusCode: statusCode);
}