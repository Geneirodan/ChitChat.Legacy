using Common;

namespace Identity.Options;

internal record AdminOptions
{
    public required string UserName { get; init; } = nameof(Roles.Admin);
    public required string Email { get; init; } = nameof(Roles.Admin);
    public required string Password { get; init; } = nameof(Roles.Admin);
}