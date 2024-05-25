using Identity.Persistence;

namespace Identity.Responses;

internal sealed record UserViewModel(string Id, string? UserName, string? Email, bool IsEmailConfirmed)
{
    public UserViewModel(User user) : this(user.Id.ToString(), user.UserName, user.Email, user.EmailConfirmed) { }
};