using System.Security.Claims;
using Common.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Common.Http;

public  sealed class HttpUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public bool IsInRole(string role) => httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;

   public Guid? Id
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var result) ? result : null;
        }
    }
}
