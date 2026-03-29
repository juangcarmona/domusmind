using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.CreateOperatorInvitation;

public sealed record CreateOperatorInvitationCommand(
    string Email,
    string? Note,
    Guid CreatedByUserId) : ICommand<CreateOperatorInvitationResponse>;
