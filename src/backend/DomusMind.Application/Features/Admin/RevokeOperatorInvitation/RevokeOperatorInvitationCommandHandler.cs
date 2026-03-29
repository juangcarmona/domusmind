using DomusMind.Application.Abstractions.Admin;
using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.Admin.RevokeOperatorInvitation;

public sealed class RevokeOperatorInvitationCommandHandler : ICommandHandler<RevokeOperatorInvitationCommand, bool>
{
    private readonly IOperatorInvitationRepository _invitations;

    public RevokeOperatorInvitationCommandHandler(IOperatorInvitationRepository invitations)
    {
        _invitations = invitations;
    }

    public async Task<bool> Handle(
        RevokeOperatorInvitationCommand command,
        CancellationToken cancellationToken)
    {
        var invitation = await _invitations.FindByIdAsync(command.InvitationId, cancellationToken);

        if (invitation is null)
            throw new AdminException(AdminErrorCode.InvitationNotFound, "Invitation not found.");

        if (invitation.Status != "Pending")
            throw new AdminException(AdminErrorCode.InvitationNotRevocable,
                $"Invitation cannot be revoked — current status is '{invitation.Status}'.");

        await _invitations.RevokeAsync(command.InvitationId, cancellationToken);

        return true;
    }
}
