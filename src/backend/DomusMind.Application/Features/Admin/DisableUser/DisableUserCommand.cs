using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.DisableUser;

public sealed record DisableUserCommand(Guid UserId) : ICommand<DisableUserResponse>;
