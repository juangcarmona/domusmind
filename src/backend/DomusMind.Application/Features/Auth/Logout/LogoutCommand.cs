using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.Logout;

public sealed record LogoutCommand(string RefreshToken) : ICommand<LogoutResponse>;
