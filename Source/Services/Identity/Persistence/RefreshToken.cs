using System.ComponentModel.DataAnnotations;

namespace Identity.Persistence;

internal sealed class RefreshToken
{
    public Guid Id { get; init; }
    [MaxLength(64)] public string Value { get; init; } = null!;
    public DateTimeOffset ExpiresAt { get; init; }
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}