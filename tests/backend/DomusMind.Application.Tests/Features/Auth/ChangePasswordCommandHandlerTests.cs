using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Application.Features.Auth.ChangePassword;
using FluentAssertions;

namespace DomusMind.Application.Tests.Features.Auth;

public sealed class ChangePasswordCommandHandlerTests
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static AuthUserRecord BuildUser(string password = "Current1!")
        => new(UserId, "user@example.com", $"HASHED:{password}");

    private static ChangePasswordCommandHandler BuildHandler(
        InMemoryAuthUserRepository? users = null,
        IRefreshTokenService? refreshTokens = null)
    {
        var repo = users ?? new InMemoryAuthUserRepository([BuildUser()]);
        return new ChangePasswordCommandHandler(
            repo,
            new StubPasswordHasher(),
            refreshTokens ?? new StubRefreshTokenService());
    }

    [Fact]
    public async Task Handle_WithValidCurrentPassword_ChangesHash()
    {
        var repo = new InMemoryAuthUserRepository([BuildUser("Current1!")]);
        var handler = BuildHandler(users: repo);

        var result = await handler.Handle(
            new ChangePasswordCommand(UserId, "Current1!", "NewSecure99!"),
            CancellationToken.None);

        result.UserId.Should().Be(UserId);
        repo.Users.Single().PasswordHash.Should().Be("HASHED:NewSecure99!");
    }

    [Fact]
    public async Task Handle_WithWrongCurrentPassword_ThrowsAuthException()
    {
        var handler = BuildHandler();

        var act = () => handler.Handle(
            new ChangePasswordCommand(UserId, "WrongCurrent!", "NewSecure99!"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.InvalidCurrentPassword);
    }

    [Fact]
    public async Task Handle_WithSamePassword_ThrowsAuthException()
    {
        var handler = BuildHandler();

        var act = () => handler.Handle(
            new ChangePasswordCommand(UserId, "Current1!", "Current1!"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.SamePassword);
    }

    [Fact]
    public async Task Handle_WithWeakNewPassword_ThrowsAuthException()
    {
        var handler = BuildHandler();

        var act = () => handler.Handle(
            new ChangePasswordCommand(UserId, "Current1!", "weak"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.WeakPassword);
    }

    [Fact]
    public async Task Handle_WithUnknownUser_ThrowsAuthException()
    {
        var handler = BuildHandler(users: new InMemoryAuthUserRepository([]));

        var act = () => handler.Handle(
            new ChangePasswordCommand(Guid.NewGuid(), "Current1!", "NewSecure99!"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.UserNotFound);
    }

    [Fact]
    public async Task Handle_RevokesAllRefreshTokensOnSuccess()
    {
        var refreshTokens = new TrackingRefreshTokenService();
        var repo = new InMemoryAuthUserRepository([BuildUser("Current1!")]);
        var handler = BuildHandler(users: repo, refreshTokens: refreshTokens);

        await handler.Handle(
            new ChangePasswordCommand(UserId, "Current1!", "NewSecure99!"),
            CancellationToken.None);

        refreshTokens.RevokedForUser.Should().Be(UserId);
    }

    // ── Test helpers ──────────────────────────────────────────────────────────

    private sealed class TrackingRefreshTokenService : IRefreshTokenService
    {
        public Guid RevokedForUser { get; private set; }

        public Task<string> CreateAsync(Guid userId, CancellationToken ct) => Task.FromResult("token");
        public Task<RotateRefreshTokenResult> ValidateAndRotateAsync(string token, CancellationToken ct)
            => Task.FromResult(new RotateRefreshTokenResult(false, null, null, null));
        public Task RevokeAsync(string token, CancellationToken ct) => Task.CompletedTask;

        public Task RevokeAllForUserAsync(Guid userId, CancellationToken ct)
        {
            RevokedForUser = userId;
            return Task.CompletedTask;
        }
    }
}
