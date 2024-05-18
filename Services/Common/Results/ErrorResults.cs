using FluentResults;

namespace Common.Results;

public static class ErrorResults
{
    public static Result NotFound() => NotFound<Result>();

    public static TResult NotFound<TResult>() where TResult : ResultBase<TResult>, new() =>
        Create<TResult>(nameof(NotFound));

    public static bool IsNotFound(this ResultBase result) => result.HasError(x => x.Message == nameof(NotFound));

    public static Result Forbidden() => Forbidden<Result>();

    public static TResult Forbidden<TResult>() where TResult : ResultBase<TResult>, new() =>
        Create<TResult>(nameof(Forbidden));

    public static bool IsForbidden(this ResultBase result) => result.HasError(x => x.Message == nameof(Forbidden));

    public static Result Unauthorized() => Unauthorized<Result>();

    public static TResult Unauthorized<TResult>() where TResult : ResultBase<TResult>, new() =>
        Create<TResult>(nameof(Unauthorized));

    public static bool IsUnauthorized(this ResultBase result) =>
        result.HasError(x => x.Message == nameof(Unauthorized));

    public static TResult Create<TResult>(params string[] errorMessages)
        where TResult : ResultBase<TResult>, new() =>
        Create<TResult>(errorMessages as IEnumerable<string>);

    public static TResult Create<TResult>(IEnumerable<string> errorMessages)
        where TResult : ResultBase<TResult>, new()
    {
        var result = new TResult();
        result.WithErrors(errorMessages);
        return result;
    }

    public static TResult Create<TResult>(params IError[] errors)
        where TResult : ResultBase<TResult>, new() =>
        Create<TResult>(errors as IEnumerable<IError>);

    public static TResult Create<TResult>(IEnumerable<IError> errors)
        where TResult : ResultBase<TResult>, new()
    {
        var result = new TResult();
        result.WithErrors(errors);
        return result;
    }
}