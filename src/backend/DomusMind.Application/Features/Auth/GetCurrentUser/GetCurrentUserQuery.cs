using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.GetCurrentUser;

public sealed record GetCurrentUserQuery(Guid UserId) : IQuery<MeResponse>;
