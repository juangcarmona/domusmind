using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Family;

namespace DomusMind.Application.Features.Family.RegenerateTemporaryPassword;

public sealed record RegenerateTemporaryPasswordCommand(
    Guid FamilyId,
    Guid MemberId,
    Guid RequestedByUserId) : ICommand<RegenerateTemporaryPasswordResponse>;
