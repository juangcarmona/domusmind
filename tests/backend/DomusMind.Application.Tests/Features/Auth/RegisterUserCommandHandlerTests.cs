using DomusMind.Application.Auth;
using DomusMind.Application.Features.Auth.RegisterUser;
using FluentAssertions;

namespace DomusMind.Application.Tests.Features.Auth;

public sealed class RegisterUserCommandHandlerTests
{
    private static RegisterUserCommandHandler BuildHandler(
        InMemoryAuthUserRepository? users = null,
        StubPasswordHasher? hasher = null)
        => new(users ?? new InMemoryAuthUserRepository(), hasher ?? new StubPasswordHasher());

    [Fact]
    public async Task Handle_WithNewEmail_CreatesUserAndReturnsResponse()
    {
        var handler = BuildHandler();

        var result = await handler.Handle(
            new RegisterUserCommand("User@Example.com", "SecurePass1!"),
            CancellationToken.None);

        result.UserId.Should().NotBeEmpty();
        result.Email.Should().Be("user@example.com"); // normalized
    }

    [Fact]
    public async Task Handle_NormalizesEmailToLowercase()
    {
        var repo = new InMemoryAuthUserRepository();
        var handler = BuildHandler(users: repo);

        await handler.Handle(
            new RegisterUserCommand("Mixed@CASE.COM", "SecurePass1!"),
            CancellationToken.None);

        var stored = repo.Users.Single();
        stored.Email.Should().Be("mixed@case.com");
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ThrowsAuthException()
    {
        var repo = new InMemoryAuthUserRepository(
            [new(Guid.NewGuid(), "existing@example.com", "hash")]);
        var handler = BuildHandler(users: repo);

        var act = () => handler.Handle(
            new RegisterUserCommand("existing@example.com", "SecurePass1!"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.EmailAlreadyTaken);
    }

    [Fact]
    public async Task Handle_WithWeakPassword_ThrowsAuthException()
    {
        var handler = BuildHandler();

        var act = () => handler.Handle(
            new RegisterUserCommand("new@example.com", "short"),
            CancellationToken.None);

        await act.Should().ThrowAsync<AuthException>()
            .Where(e => e.Code == AuthErrorCode.WeakPassword);
    }

    [Fact]
    public async Task Handle_HashesPassword_BeforeStoring()
    {
        var repo = new InMemoryAuthUserRepository();
        var handler = BuildHandler(users: repo);

        await handler.Handle(
            new RegisterUserCommand("user@example.com", "SecurePass1!"),
            CancellationToken.None);

        repo.Users.Single().PasswordHash.Should().StartWith("HASHED:");
    }
}
