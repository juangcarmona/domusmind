using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Family;

namespace DomusMind.Application.Features.Family.LinkMemberAccount;

public sealed record LinkMemberAccountCommand(
    Guid FamilyId,
    Guid MemberId,
    string Username,
    string TemporaryPassword,
    Guid RequestedByUserId) : ICommand<LinkMemberAccountResponse>;
