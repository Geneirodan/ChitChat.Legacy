using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Identity.Extensions;

internal static class IdentityResultExtensions
{
    internal static ValidationProblem ToValidationProblem(this IdentityResult result)
    {
        var errorsDict = result.Errors
            .GroupBy(x => x.Code, x => x.Description)
            .ToDictionary(x => x.Key, x => x.ToArray());
        return TypedResults.ValidationProblem(errorsDict);
    }
}