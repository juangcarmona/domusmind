using DomusMind.Application.Abstractions.Admin;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.CreateOperatorInvitation;

public sealed class CreateOperatorInvitationCommandHandler
    : ICommandHandler<CreateOperatorInvitationCommand, CreateOperatorInvitationResponse>
{
    private readonly IOperatorInvitationRepository _invitations;

    public CreateOperatorInvitationCommandHandler(IOperatorInvitationRepository invitations)
    {
        _invitations = invitations;
    }

    public async Task<CreateOperatorInvitationResponse> Handle(
        CreateOperatorInvitationCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(email))
            throw new AdminException(AdminErrorCode.InvalidInput, "Email is required.");

        var result = await _invitations.CreateAsync(
            email, command.Note, command.CreatedByUserId, cancellationToken);

        return new CreateOperatorInvitationResponse(
            result.Invitation.Id,
            result.Invitation.Email,
            result.Token,
            result.Invitation.ExpiresAtUtc);
    }
}
