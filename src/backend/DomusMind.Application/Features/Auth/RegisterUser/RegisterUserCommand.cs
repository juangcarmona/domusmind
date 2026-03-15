using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Contracts.Auth;

namespace DomusMind.Application.Features.Auth.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password) : ICommand<RegisterUserResponse>;
