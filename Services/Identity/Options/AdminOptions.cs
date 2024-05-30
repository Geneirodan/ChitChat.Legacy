namespace Identity.Options;

internal sealed record AdminOptions
{
    public required string UserName { get; init; } 
    public required string Email { get; init; } 
    public required string Password { get; init; }
}