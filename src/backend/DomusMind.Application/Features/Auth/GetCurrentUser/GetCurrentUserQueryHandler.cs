using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, MeResponse>
{
    private readonly IAuthUserRepository _users;

    public GetCurrentUserQueryHandler(IAuthUserRepository users)
    {
        _users = users;
    }

    public async Task<MeResponse> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _users.FindByIdAsync(query.UserId, cancellationToken);
        if (user is null)
            throw new AuthException(AuthErrorCode.UserNotFound, "Authenticated user not found.");

        return new MeResponse(user.UserId, user.Email);
    }
}
