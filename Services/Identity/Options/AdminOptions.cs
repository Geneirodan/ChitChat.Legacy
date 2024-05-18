using static Common.Other.Roles;

namespace Identity.Options;

internal record AdminOptions
{
    public required string UserName { get; init; } = nameof(Admin);
    public required string Email { get; init; } = nameof(Admin);
    public required string Password { get; init; } = nameof(Admin);
}