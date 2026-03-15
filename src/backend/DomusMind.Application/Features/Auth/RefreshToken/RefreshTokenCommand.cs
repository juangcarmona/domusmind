using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string Token) : ICommand<RefreshTokenResponse>;
