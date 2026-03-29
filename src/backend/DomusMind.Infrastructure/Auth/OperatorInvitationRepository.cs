using System.Security.Cryptography;
using DomusMind.Application.Abstractions.Admin;
using DomusMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Infrastructure.Auth;

public sealed class OperatorInvitationRepository : IOperatorInvitationRepository
{
    private readonly DomusMindDbContext _db;

    public OperatorInvitationRepository(DomusMindDbContext db)
    {
        _db = db;
    }

    public async Task<int> CountPendingAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _db.OperatorInvitations
            .CountAsync(i => i.Status == "Pending" && i.ExpiresAtUtc > now, cancellationToken);
    }

    public async Task<IReadOnlyList<OperatorInvitationProjection>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _db.OperatorInvitations
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAtUtc)
            .Select(i => new OperatorInvitationProjection(
                i.Id,
                i.Email,
                i.Note,
                i.Status == "Pending" && i.ExpiresAtUtc <= now ? "Expired" : i.Status,
                i.CreatedAtUtc,
                i.ExpiresAtUtc,
                i.CreatedByUserId))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperatorInvitationProjection?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var i = await _db.OperatorInvitations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (i is null) return null;

        return new OperatorInvitationProjection(
            i.Id,
            i.Email,
            i.Note,
            i.Status == "Pending" && i.ExpiresAtUtc <= now ? "Expired" : i.Status,
            i.CreatedAtUtc,
            i.ExpiresAtUtc,
            i.CreatedByUserId);
    }

    public async Task<OperatorInvitationCreatedResult> CreateAsync(
        string email,
        string? note,
        Guid createdByUserId,
        CancellationToken cancellationToken = default)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-").Replace("/", "_").Replace("=", "");

        var now = DateTime.UtcNow;
        var invitation = new OperatorInvitation
        {
            Id = Guid.NewGuid(),
            Email = email,
            Note = note?.Trim(),
            Token = token,
            Status = "Pending",
            CreatedAtUtc = now,
            ExpiresAtUtc = now.AddDays(7),
            CreatedByUserId = createdByUserId,
        };

        _db.OperatorInvitations.Add(invitation);
        await _db.SaveChangesAsync(cancellationToken);

        var projection = new OperatorInvitationProjection(
            invitation.Id,
            invitation.Email,
            invitation.Note,
            invitation.Status,
            invitation.CreatedAtUtc,
            invitation.ExpiresAtUtc,
            invitation.CreatedByUserId);

        return new OperatorInvitationCreatedResult(projection, token);
    }

    public async Task RevokeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invitation = await _db.OperatorInvitations
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (invitation is null)
            throw new InvalidOperationException($"Invitation {id} not found.");

        invitation.Status = "Revoked";
        await _db.SaveChangesAsync(cancellationToken);
    }
}
