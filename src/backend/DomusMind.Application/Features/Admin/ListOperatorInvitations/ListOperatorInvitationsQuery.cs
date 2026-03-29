using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.ListOperatorInvitations;

public sealed record ListOperatorInvitationsQuery() : IQuery<OperatorInvitationListResponse>;
