using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Family;
using DomusMind.Domain.Family.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.LinkMemberAccount;

public sealed class LinkMemberAccountCommandHandler
    : ICommandHandler<LinkMemberAccountCommand, LinkMemberAccountResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IEventLogWriter _eventLogWriter;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IFamilyAccessGranter _familyAccessGranter;

    public LinkMemberAccountCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter,
        IFamilyAuthorizationService authorizationService,
        IAuthUserRepository authUserRepository,
        IPasswordHasher passwordHasher,
        IFamilyAccessGranter familyAccessGranter)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
        _authorizationService = authorizationService;
        _authUserRepository = authUserRepository;
        _passwordHasher = passwordHasher;
        _familyAccessGranter = familyAccessGranter;
    }

    public async Task<LinkMemberAccountResponse> Handle(
        LinkMemberAccountCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            throw new FamilyException(FamilyErrorCode.InvalidInput, "Username is required.");

        if (string.IsNullOrWhiteSpace(command.TemporaryPassword) || command.TemporaryPassword.Length < 6)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "Temporary password must be at least 6 characters.");

        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            command.RequestedByUserId, command.FamilyId, cancellationToken);

        if (!canAccess)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Access to this family is denied.");

        var family = await _dbContext.Set<Domain.Family.Family>()
            .Include(f => f.Members)
            .SingleOrDefaultAsync(f => f.Id == FamilyId.From(command.FamilyId), cancellationToken);

        if (family is null)
            throw new FamilyException(FamilyErrorCode.FamilyNotFound, "Family was not found.");

        // Ensure the requesting user is a manager
        var requestingMember = family.Members.FirstOrDefault(m => m.AuthUserId == command.RequestedByUserId);
        if (requestingMember is null || !requestingMember.IsManager)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Only household managers can link accounts to members.");

        var memberId = MemberId.From(command.MemberId);
        var targetMember = family.Members.FirstOrDefault(m => m.Id == memberId);
        if (targetMember is null)
            throw new FamilyException(FamilyErrorCode.MemberNotFound, "Member was not found.");

        if (targetMember.AuthUserId.HasValue)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "This member already has a linked account.");

        var username = command.Username.Trim().ToLowerInvariant();

        var existingAuthUser = await _authUserRepository.FindByEmailAsync(username, cancellationToken);
        if (existingAuthUser is not null)
            throw new FamilyException(FamilyErrorCode.InvalidInput, "The username is already taken.");

        var authUserId = Guid.NewGuid();
        var passwordHash = _passwordHasher.Hash(command.TemporaryPassword);
        var authUserRecord = new AuthUserRecord(authUserId, username, passwordHash, MustChangePassword: true);
        await _authUserRepository.AddAsync(authUserRecord, cancellationToken);

        var now = DateTime.UtcNow;

        try
        {
            family.LinkMemberAccount(memberId, authUserId, now);
        }
        catch (InvalidOperationException ex)
        {
            throw new FamilyException(FamilyErrorCode.InvalidInput, ex.Message);
        }

        await _familyAccessGranter.GrantAccessAsync(authUserId, command.FamilyId, cancellationToken);

        // Persist the auth user and domain changes before writing events
        // so that if persistence fails no events are emitted.
        await _authUserRepository.SaveChangesAsync(cancellationToken);

        await _eventLogWriter.WriteAsync(family.DomainEvents, cancellationToken);
        family.ClearDomainEvents();

        return new LinkMemberAccountResponse(
            targetMember.Id.Value,
            family.Id.Value,
            targetMember.Name.Value,
            targetMember.Role.Value,
            targetMember.IsManager,
            targetMember.BirthDate,
            username,
            authUserId,
            now);
    }
}
