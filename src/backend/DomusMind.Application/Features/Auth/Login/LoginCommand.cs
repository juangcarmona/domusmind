using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
