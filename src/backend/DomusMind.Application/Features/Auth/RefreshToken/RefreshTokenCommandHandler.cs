using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenService _refreshTokens;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokens,
        IAccessTokenGenerator tokenGenerator)
    {
        _refreshTokens = refreshTokens;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _refreshTokens.ValidateAndRotateAsync(command.Token, cancellationToken);

        if (!result.IsValid || result.UserId is null || result.UserEmail is null)
            throw new AuthException(AuthErrorCode.InvalidRefreshToken, "Refresh token is invalid or expired.");

        var accessToken = _tokenGenerator.Generate(result.UserId.Value, result.UserEmail);

        return new RefreshTokenResponse(accessToken, result.NewToken!);
    }
}
