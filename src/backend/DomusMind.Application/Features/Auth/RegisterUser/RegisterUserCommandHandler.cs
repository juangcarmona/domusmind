using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.RegisterUser;

public sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IAuthUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public RegisterUserCommandHandler(IAuthUserRepository users, IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<RegisterUserResponse> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 8)
            throw new AuthException(AuthErrorCode.WeakPassword, "Password must be at least 8 characters.");

        var existing = await _users.FindByEmailAsync(email, cancellationToken);
        if (existing is not null)
            throw new AuthException(AuthErrorCode.EmailAlreadyTaken, "Email address is already registered.");

        var user = new AuthUserRecord(Guid.NewGuid(), email, _hasher.Hash(command.Password));

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponse(user.UserId, user.Email);
    }
}
