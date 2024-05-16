using System.Reflection;
using Common.Abstractions;
using Common.Mediator.Attributes;
using Common.Results;
using FluentResults;
using MediatR;

namespace Common.Mediator.Behaviors;

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
            return ErrorResults.Unauthorized<TResponse>();

        var authorizeAttributesWithRoles =
            authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles)).ToArray();

        if (authorizeAttributesWithRoles.Length == 0)
            return await next().ConfigureAwait(false);
        
        var authorized = authorizeAttributesWithRoles.SelectMany(a => a.Roles.Split(','))
            .Any(role => user.IsInRole(role.Trim()));

        return authorized 
            ? await next().ConfigureAwait(false) 
            : ErrorResults.Forbidden<TResponse>();
    }
}