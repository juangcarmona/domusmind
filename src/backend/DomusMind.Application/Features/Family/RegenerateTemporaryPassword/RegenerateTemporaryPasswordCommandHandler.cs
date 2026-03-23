using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.RegenerateTemporaryPassword;

/// <summary>
/// Admin-only: generates a new temporary password for a member's existing account.
/// The new password is returned once in the response and all existing sessions are revoked.
/// </summary>
public sealed class RegenerateTemporaryPasswordCommandHandler
    : ICommandHandler<RegenerateTemporaryPasswordCommand, RegenerateTemporaryPasswordResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITemporaryPasswordGenerator _passwordGenerator;
    private readonly IRefreshTokenService _refreshTokens;

    public RegenerateTemporaryPasswordCommandHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService,
        IAuthUserRepository authUserRepository,
        IPasswordHasher passwordHasher,
        ITemporaryPasswordGenerator passwordGenerator,
        IRefreshTokenService refreshTokens)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _authUserRepository = authUserRepository;
        _passwordHasher = passwordHasher;
        _passwordGenerator = passwordGenerator;
        _refreshTokens = refreshTokens;
    }

    public async Task<RegenerateTemporaryPasswordResponse> Handle(
        RegenerateTemporaryPasswordCommand command,
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
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Only household managers can regenerate passwords.");

        var memberId = MemberId.From(command.MemberId);
        var targetMember = family.Members.FirstOrDefault(m => m.Id == memberId);
        if (targetMember is null)
            throw new FamilyException(FamilyErrorCode.MemberNotFound, "Member was not found.");

        if (!targetMember.AuthUserId.HasValue)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "This member does not have a linked account.");

        var linkedUserId = targetMember.AuthUserId.Value;

        var temporaryPassword = _passwordGenerator.Generate();
        var newHash = _passwordHasher.Hash(temporaryPassword);

        await _authUserRepository.UpdatePasswordHashAsync(linkedUserId, newHash, cancellationToken);
        await _authUserRepository.UpdateMustChangePasswordAsync(linkedUserId, true, cancellationToken);

        // Invalidate all existing sessions for that user
        await _refreshTokens.RevokeAllForUserAsync(linkedUserId, cancellationToken);

        await _authUserRepository.SaveChangesAsync(cancellationToken);

        return new RegenerateTemporaryPasswordResponse(temporaryPassword, MustChangePassword: true);
    }
}
