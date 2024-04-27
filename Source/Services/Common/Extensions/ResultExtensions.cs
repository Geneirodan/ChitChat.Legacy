using FluentResults;

namespace Common.Extensions;

public static class ResultExtensions
{
    public static TResult WithReason<TResult>(this ResultBase<TResult> result, string reason) 
        where TResult : ResultBase<TResult> => result.WithReason(new Error(reason));
}