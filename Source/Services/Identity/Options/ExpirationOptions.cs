namespace Identity.Options;

internal sealed record ExpirationOptions
{
    /// <summary>
    /// Lifetime of AccessToken in minutes. Default is 15 min.
    /// </summary>
    public required int AccessToken { get; init; } = 15;
    
    
    /// <summary>
    /// Lifetime of RefreshToken in minutes. Default is 86400 min = 60 days.
    /// </summary>
    public required int RefreshToken { get; init; } = 86400;
}