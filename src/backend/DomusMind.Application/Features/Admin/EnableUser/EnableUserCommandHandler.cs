using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.EnableUser;

public sealed class EnableUserCommandHandler : ICommandHandler<EnableUserCommand, EnableUserResponse>
{
    private readonly IAuthUserRepository _users;

    public EnableUserCommandHandler(IAuthUserRepository users)
    {
        _users = users;
    }

    public async Task<EnableUserResponse> Handle(
        EnableUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _users.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            throw new AdminException(AdminErrorCode.UserNotFound, "User not found.");

        await _users.EnableUserAsync(command.UserId, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        return new EnableUserResponse(command.UserId, IsDisabled: false);
    }
}
