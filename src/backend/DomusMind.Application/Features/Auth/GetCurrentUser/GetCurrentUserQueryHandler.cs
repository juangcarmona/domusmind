using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Auth;
using DomusMind.Domain.Family;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.Auth.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, MeResponse>
{
    private readonly IAuthUserRepository _users;
    private readonly IDomusMindDbContext _dbContext;

    public GetCurrentUserQueryHandler(IAuthUserRepository users, IDomusMindDbContext dbContext)
    {
        _users = users;
        _dbContext = dbContext;
    }

    public async Task<MeResponse> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.FindByIdAsync(query.UserId, cancellationToken);
        if (user is null)
            throw new AuthException(AuthErrorCode.UserNotFound, "Authenticated user not found.");

        string? memberName = null;
        Guid? memberId = null;
        bool isManager = false;

        // Resolve linked member for name and role, using MemberId stored on auth user
        // or by scanning FamilyMember.AuthUserId as fallback for older accounts.
        var linkedMemberId = user.MemberId;
        if (linkedMemberId.HasValue)
        {
            var member = await _dbContext.Set<FamilyMember>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == MemberId.From(linkedMemberId.Value), cancellationToken);

            if (member is not null)
            {
                memberName = member.Name.Value;
                memberId = member.Id.Value;
                isManager = member.IsManager;
            }
        }
        else
        {
            // Legacy path: auth users created before MemberId was introduced
            var member = await _dbContext.Set<FamilyMember>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AuthUserId == query.UserId, cancellationToken);

            if (member is not null)
            {
                memberName = member.Name.Value;
                memberId = member.Id.Value;
                isManager = member.IsManager;
            }
        }

        return new MeResponse(
            user.UserId,
            user.Email,
            user.DisplayName,
            memberId,
            memberName,
            isManager,
            user.MustChangePassword);
    }
}

