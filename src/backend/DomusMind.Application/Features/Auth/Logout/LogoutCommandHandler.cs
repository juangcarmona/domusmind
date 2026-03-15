using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.Logout;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, LogoutResponse>
{
    private readonly IRefreshTokenService _refreshTokens;

    public LogoutCommandHandler(IRefreshTokenService refreshTokens)
    {
        _refreshTokens = refreshTokens;
    }

    public async Task<LogoutResponse> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        // Best-effort revocation. If the token is already expired / unknown, we still succeed.
        await _refreshTokens.RevokeAsync(command.RefreshToken, cancellationToken);
        return new LogoutResponse();
    }
}
