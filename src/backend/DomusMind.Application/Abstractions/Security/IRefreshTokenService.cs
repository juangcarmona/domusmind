namespace DomusMind.Application.Abstractions.Security;

/// <summary>
/// Result of a refresh-token rotation operation.
/// </summary>
public sealed record RotateRefreshTokenResult(bool IsValid, string? NewToken, Guid? UserId, string? UserEmail);

public interface IRefreshTokenService
{
    /// <summary>Creates a new refresh token for the given user and persists it.</summary>
    Task<string> CreateAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Validates the presented token, revokes it, and issues a new one.
    /// Returns IsValid=false when the token is unknown, expired, or already revoked.
    /// </summary>
    Task<RotateRefreshTokenResult> ValidateAndRotateAsync(string token, CancellationToken cancellationToken);

    /// <summary>Revokes the specific token (logout).</summary>
    Task RevokeAsync(string token, CancellationToken cancellationToken);

    /// <summary>Revokes all active refresh tokens for the user (change password, security events).</summary>
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken);
}
