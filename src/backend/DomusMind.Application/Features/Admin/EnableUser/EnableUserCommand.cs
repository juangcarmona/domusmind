using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Admin;

namespace DomusMind.Application.Features.Admin.EnableUser;

public sealed record EnableUserCommand(Guid UserId) : ICommand<EnableUserResponse>;
