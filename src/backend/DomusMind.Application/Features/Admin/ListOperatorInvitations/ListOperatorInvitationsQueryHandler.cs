using DomusMind.Application.Abstractions.Admin;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.ListOperatorInvitations;

public sealed class ListOperatorInvitationsQueryHandler
    : IQueryHandler<ListOperatorInvitationsQuery, OperatorInvitationListResponse>
{
    private readonly IOperatorInvitationRepository _invitations;

    public ListOperatorInvitationsQueryHandler(IOperatorInvitationRepository invitations)
    {
        _invitations = invitations;
    }

    public async Task<OperatorInvitationListResponse> Handle(
        ListOperatorInvitationsQuery query,
        CancellationToken cancellationToken)
    {
        var all = await _invitations.GetAllAsync(cancellationToken);

        var items = all.Select(i => new OperatorInvitationItem(
            i.Id, i.Email, i.Note, i.Status, i.CreatedAtUtc, i.ExpiresAtUtc, i.CreatedByUserId))
            .ToList();

        return new OperatorInvitationListResponse(items);
    }
}
