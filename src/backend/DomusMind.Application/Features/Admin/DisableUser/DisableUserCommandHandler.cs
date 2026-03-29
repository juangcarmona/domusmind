using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.DisableUser;

public sealed class DisableUserCommandHandler : ICommandHandler<DisableUserCommand, DisableUserResponse>
{
    private readonly IAuthUserRepository _users;

    public DisableUserCommandHandler(IAuthUserRepository users)
    {
        _users = users;
    }

    public async Task<DisableUserResponse> Handle(
        DisableUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _users.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            throw new AdminException(AdminErrorCode.UserNotFound, "User not found.");

        if (user.IsOperator)
            throw new AdminException(AdminErrorCode.CannotDisableOperator, "Operator accounts cannot be disabled.");

        await _users.DisableUserAsync(command.UserId, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        return new DisableUserResponse(command.UserId, IsDisabled: true);
    }
}
