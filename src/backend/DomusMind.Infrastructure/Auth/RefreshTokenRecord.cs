namespace DomusMind.Infrastructure.Auth;

/// <summary>
/// Refresh token persistence entity. Tokens are stored as SHA-256 hashes.
/// </summary>
public sealed class RefreshTokenRecord
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    /// <summary>SHA-256 hash of the opaque refresh token returned to the client.</summary>
    public string TokenHash { get; init; } = default!;

    public DateTime ExpiresAtUtc { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public bool IsRevoked { get; set; }

    public DateTime? RevokedAtUtc { get; set; }
}
