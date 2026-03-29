using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.GetAdminUsers;

public sealed class GetAdminUsersQueryHandler : IQueryHandler<GetAdminUsersQuery, AdminUserListResponse>
{
    private readonly IAuthUserRepository _users;
    private readonly IUserFamilyAccessReader _familyAccess;

    public GetAdminUsersQueryHandler(IAuthUserRepository users, IUserFamilyAccessReader familyAccess)
    {
        _users = users;
        _familyAccess = familyAccess;
    }

    public async Task<AdminUserListResponse> Handle(
        GetAdminUsersQuery query,
        CancellationToken cancellationToken)
    {
        var projections = await _users.GetAdminProjectionsAsync(query.Search, cancellationToken);
        var familyMap = await _familyAccess.GetAllFamilyIdsByUserAsync(cancellationToken);

        var items = projections
            .Select(u => new AdminUserSummary(
                UserId: u.UserId,
                Email: u.Email,
                DisplayName: u.DisplayName,
                IsDisabled: u.IsDisabled,
                IsOperator: u.IsOperator,
                CreatedAtUtc: u.CreatedAtUtc,
                LastLoginAtUtc: u.LastLoginAtUtc,
                LinkedFamilyId: familyMap.TryGetValue(u.UserId, out var fid) ? fid : null))
            .ToList();

        return new AdminUserListResponse(items);
    }
}
