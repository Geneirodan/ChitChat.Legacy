namespace Identity.Options;

internal sealed record EmailOptions
{
    public required string SenderName { get; init; }
    public required string DefaultFromEmail { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string TemplatesFolder { get; init; } = "Templates";
}