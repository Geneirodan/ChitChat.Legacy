using System.Reflection;
using Common.Interfaces;
using Common.MediatR.Attributes;
using FluentResults;
using MediatR;
using static System.Net.HttpStatusCode;

namespace Common.MediatR.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse>(IUser user) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ResultBase<TResponse>, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToArray();

        if (authorizeAttributes.Length == 0)
            return await next().ConfigureAwait(false);

        if (user is { Id: null })
            return new TResponse().WithError(nameof(Unauthorized));

        var authorizeAttributesWithRoles =
            authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles)).ToArray();

        if (authorizeAttributesWithRoles.Length == 0)
            return await next().ConfigureAwait(false);
        
        var authorized = authorizeAttributesWithRoles.SelectMany(a => a.Roles.Split(','))
            .Any(role => user.IsInRole(role.Trim()));

        return authorized ? await next().ConfigureAwait(false) : new TResponse().WithError(nameof(Forbidden));
    }
}