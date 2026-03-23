using DomusMind.Api.Controllers;
using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Tests.Controllers;

public sealed class AuthControllerTests
{
    [Fact]
    public void Health_ReturnsOk_WithStatusOk()
    {
        var sut = new AuthController(new StubCurrentUser(null, null));

        var result = sut.Health();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Me_WhenUserIdIsNull_ReturnsUnauthorized()
    {
        var sut = new AuthController(new StubCurrentUser(null, null));

        var result = await sut.Me(new StubQueryDispatcher(null), CancellationToken.None);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Me_WhenUserIdPresent_ReturnsOkWithMeResponse()
    {
        var userId = Guid.NewGuid();
        var email = "user@example.com";
        var dispatcher = new StubQueryDispatcher(new MeResponse(userId, email, null, null, null, false, false));
        var sut = new AuthController(new StubCurrentUser(userId, email));

        var result = await sut.Me(dispatcher, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var me = ok.Value.Should().BeOfType<MeResponse>().Subject;
        me.UserId.Should().Be(userId);
        me.Email.Should().Be(email);
    }

    [Fact]
    public async Task Me_WhenEmailIsNull_ReturnsOkWithNullEmail()
    {
        var userId = Guid.NewGuid();
        var dispatcher = new StubQueryDispatcher(new MeResponse(userId, null, null, null, null, false, false));
        var sut = new AuthController(new StubCurrentUser(userId, null));

        var result = await sut.Me(dispatcher, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var me = ok.Value.Should().BeOfType<MeResponse>().Subject;
        me.UserId.Should().Be(userId);
        me.Email.Should().BeNull();
    }

    private sealed class StubCurrentUser : ICurrentUser
    {
        private readonly Guid? _userId;
        private readonly string? _email;

        public StubCurrentUser(Guid? userId, string? email)
        {
            _userId = userId;
            _email = email;
        }

        public Guid? UserId => _userId;
        public string? Email => _email;
        public bool IsAuthenticated => _userId.HasValue;
        public IReadOnlyCollection<string> Roles => [];
    }

    private sealed class StubQueryDispatcher : IQueryDispatcher
    {
        private readonly object? _response;

        public StubQueryDispatcher(object? response)
        {
            _response = response;
        }

        public Task<TResponse> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            if (_response is TResponse typed)
                return Task.FromResult(typed);

            return Task.FromResult(default(TResponse)!);
        }
    }
}
