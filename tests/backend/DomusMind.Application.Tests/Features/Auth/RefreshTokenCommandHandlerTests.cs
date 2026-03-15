using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Application.Features.Auth.RefreshToken;
using FluentAssertions;

namespace DomusMind.Application.Tests.Features.Auth;

public sealed class RefreshTokenCommandHandlerTests
{
    private static readonly Guid UserId = Guid.NewGuid();
    private const string UserEmail = "user@example.com";

    private static RefreshTokenCommandHandler BuildHandler(IRefreshTokenService? refreshTokens = null)
        => new(refreshTokens ?? new ValidStubRefreshTokenService(), new StubAccessTokenGenerator());

    [Fact]
    public async Task Handle_WithValidToken_ReturnsNewTokenPair()
    {
        var handler = BuildHandler();

        var result = await handler.Handle(
            new RefreshTokenCommand("valid-refresh-token"),
            CancellationToken.None);

        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Handle_WithInvalidToken_ThrowsAuthException()
    {
        var handler = BuildHandler(new InvalidStubRefreshTokenService());

        var act = () => handler.Handle(
            new RefreshTokenCommand("invalid-or-expired-token"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.InvalidRefreshToken);
    }

    // ── Test helpers ──────────────────────────────────────────────────────────

    private sealed class ValidStubRefreshTokenService : IRefreshTokenService
    {
        public Task<string> CreateAsync(Guid userId, CancellationToken ct)
            => Task.FromResult("new-refresh-token");

        public Task<RotateRefreshTokenResult> ValidateAndRotateAsync(string token, CancellationToken ct)
            => Task.FromResult(new RotateRefreshTokenResult(true, "new-refresh-token", UserId, UserEmail));

        public Task RevokeAsync(string token, CancellationToken ct) => Task.CompletedTask;
        public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct) => Task.CompletedTask;
    }

    private sealed class InvalidStubRefreshTokenService : IRefreshTokenService
    {
        public Task<string> CreateAsync(Guid userId, CancellationToken ct)
            => Task.FromResult("token");

        public Task<RotateRefreshTokenResult> ValidateAndRotateAsync(string token, CancellationToken ct)
            => Task.FromResult(new RotateRefreshTokenResult(false, null, null, null));

        public Task RevokeAsync(string token, CancellationToken ct) => Task.CompletedTask;
        public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct) => Task.CompletedTask;
    }
}
