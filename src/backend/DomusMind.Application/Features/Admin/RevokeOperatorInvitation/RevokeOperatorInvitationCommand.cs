using DomusMind.Application.Abstractions.Messaging;

namespace DomusMind.Application.Features.Admin.RevokeOperatorInvitation;

public sealed record RevokeOperatorInvitationCommand(Guid InvitationId) : ICommand<bool>;
