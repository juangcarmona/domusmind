using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.Family;
using DomusMind.Contracts.Family;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Family.GetFamilyMembers;

public sealed class GetFamilyMembersQueryHandler
    : IQueryHandler<GetFamilyMembersQuery, IReadOnlyCollection<FamilyMemberResponse>>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;
    private readonly IAuthUserRepository _authUserRepository;

    public GetFamilyMembersQueryHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService,
        IAuthUserRepository authUserRepository)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _authUserRepository = authUserRepository;
    }

    public async Task<IReadOnlyCollection<FamilyMemberResponse>> Handle(
        GetFamilyMembersQuery query,
        CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            query.RequestedByUserId, query.FamilyId, cancellationToken);

        if (!canAccess)
            throw new FamilyException(FamilyErrorCode.AccessDenied, "Access to this family is denied.");

        var family = await _dbContext.Set<Domain.Family.Family>()
            .AsNoTracking()
            .Include(f => f.Members)
            .SingleOrDefaultAsync(f => f.Id == FamilyId.From(query.FamilyId), cancellationToken);

        if (family is null)
            throw new FamilyException(FamilyErrorCode.FamilyNotFound, "Family was not found.");

        var linkedUserIds = family.Members
            .Where(m => m.AuthUserId.HasValue)
            .Select(m => m.AuthUserId!.Value)
            .ToList();

        var authStatus = await _authUserRepository.GetStatusByIdsAsync(linkedUserIds, cancellationToken);

        return family.Members
            .OrderBy(m => m.Role.Value switch { "Adult" => 0, "Child" => 1, "Pet" => 2, _ => 3 })
            .ThenByDescending(m => m.IsManager)
            .ThenBy(m => m.Name.Value, StringComparer.OrdinalIgnoreCase)
            .Select(m =>
            {
                MemberAccessStatus status;
                string? linkedEmail = null;

                if (!m.AuthUserId.HasValue)
                {
                    status = MemberAccessStatus.None;
                }
                else if (authStatus.TryGetValue(m.AuthUserId.Value, out var projection))
                {
                    linkedEmail = projection.Email;
                    status = projection.IsDisabled
                        ? MemberAccessStatus.Disabled
                        : projection.MustChangePassword
                            ? MemberAccessStatus.PasswordChangeRequired
                            : MemberAccessStatus.Active;
                }
                else
                {
                    status = MemberAccessStatus.None;
                }

                return new FamilyMemberResponse(
                    m.Id.Value,
                    family.Id.Value,
                    m.Name.Value,
                    m.Role.Value,
                    m.IsManager,
                    m.BirthDate,
                    m.JoinedAtUtc,
                    m.AuthUserId,
                    status,
                    linkedEmail);
            })
            .ToList()
            .AsReadOnly();
    }
}

