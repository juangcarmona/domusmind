using System.Security.Cryptography;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DomusMind.Infrastructure.Auth;

public sealed class RefreshTokenStore : IRefreshTokenService
{
    private readonly DomusMindDbContext _db;
    private readonly JwtOptions _options;

    public RefreshTokenStore(DomusMindDbContext db, IOptions<JwtOptions> options)
    {
        _db = db;
        _options = options.Value;
    }

    public async Task<string> CreateAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = GenerateToken();
        var hash = HashToken(token);

        var record = new RefreshTokenRecord
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = hash,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiryDays),
            IsRevoked = false,
        };

        _db.Set<RefreshTokenRecord>().Add(record);
        await _db.SaveChangesAsync(cancellationToken);

        return token;
    }

    public async Task<RotateRefreshTokenResult> ValidateAndRotateAsync(
        string token,
        CancellationToken cancellationToken)
    {
        var hash = HashToken(token);

        var record = await _db.Set<RefreshTokenRecord>()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.TokenHash == hash, cancellationToken);

        if (record is null || record.IsRevoked || record.ExpiresAtUtc < DateTime.UtcNow)
            return new RotateRefreshTokenResult(false, null, null, null);

        // Fetch the user email for the new access token
        var user = await _db.Set<AuthUser>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == record.UserId, cancellationToken);

        if (user is null)
            return new RotateRefreshTokenResult(false, null, null, null);

        // Revoke the old record
        await _db.Set<RefreshTokenRecord>()
            .Where(r => r.TokenHash == hash)
            .ExecuteUpdateAsync(
                s => s.SetProperty(r => r.IsRevoked, true)
                       .SetProperty(r => r.RevokedAtUtc, DateTime.UtcNow),
                cancellationToken);

        // Issue a new token
        var newToken = GenerateToken();
        var newHash = HashToken(newToken);

        var newRecord = new RefreshTokenRecord
        {
            Id = Guid.NewGuid(),
            UserId = record.UserId,
            TokenHash = newHash,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiryDays),
            IsRevoked = false,
        };

        _db.Set<RefreshTokenRecord>().Add(newRecord);
        await _db.SaveChangesAsync(cancellationToken);

        return new RotateRefreshTokenResult(true, newToken, record.UserId, user.Email);
    }

    public async Task RevokeAsync(string token, CancellationToken cancellationToken)
    {
        var hash = HashToken(token);

        await _db.Set<RefreshTokenRecord>()
            .Where(r => r.TokenHash == hash && !r.IsRevoked)
            .ExecuteUpdateAsync(
                s => s.SetProperty(r => r.IsRevoked, true)
                       .SetProperty(r => r.RevokedAtUtc, DateTime.UtcNow),
                cancellationToken);
    }

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _db.Set<RefreshTokenRecord>()
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ExecuteUpdateAsync(
                s => s.SetProperty(r => r.IsRevoked, true)
                       .SetProperty(r => r.RevokedAtUtc, DateTime.UtcNow),
                cancellationToken);
    }

    private static string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(48);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    private static string HashToken(string token)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
