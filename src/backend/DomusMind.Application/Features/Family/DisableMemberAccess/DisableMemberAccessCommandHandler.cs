using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.DisableMemberAccess;

/// <summary>
/// Admin-only: disables a member's auth account so they can no longer log in.
/// All existing refresh tokens for that user are revoked immediately.
/// </summary>
public sealed class DisableMemberAccessCommandHandler
    : ICommandHandler<DisableMemberAccessCommand, DisableMemberAccessResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IRefreshTokenService _refreshTokens;

    public DisableMemberAccessCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService,
        IAuthUserRepository authUserRepository,
        IRefreshTokenService refreshTokens)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _authUserRepository = authUserRepository;
        _refreshTokens = refreshTokens;
    }

    public async Task<DisableMemberAccessResponse> Handle(
        DisableMemberAccessCommand command,
        CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);

        if (!canAccess)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Access to this family is denied.");

        var family = await _dbContext.Set<Domain.Family.Family>()
            .AsNoTracking()
            .Include(f => f.Members)
            .SingleOrDefaultAsync(f => f.Id == FamilyId.From(command.FamilyId), cancellationToken);

        if (family is null)
            throw new FamilyException(FamilyErrorCode.FamilyNotFound, "Family was not found.");

        var requestingMember = family.Members.FirstOrDefault(m => m.AuthUserId == command.RequestedByUserId);
        if (requestingMember is null || !requestingMember.IsManager)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Only household managers can disable member access.");

        var memberId = MemberId.From(command.MemberId);
        var targetMember = family.Members.FirstOrDefault(m => m.Id == memberId);
        if (targetMember is null)
            throw new FamilyException(FamilyErrorCode.MemberNotFound, "Member was not found.");

        if (!targetMember.AuthUserId.HasValue)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "This member does not have a linked account.");

        var linkedUserId = targetMember.AuthUserId.Value;

        await _authUserRepository.DisableUserAsync(linkedUserId, cancellationToken);

        // Revoke all sessions immediately so in-flight tokens stop working at next refresh
        await _refreshTokens.RevokeAllForUserAsync(linkedUserId, cancellationToken);

        await _authUserRepository.SaveChangesAsync(cancellationToken);

        return new DisableMemberAccessResponse(command.MemberId);
    }
}
