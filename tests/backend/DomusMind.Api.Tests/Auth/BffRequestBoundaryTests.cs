using DomusMind.Api.Auth;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace DomusMind.Api.Tests.Auth;

public sealed class BffRequestBoundaryTests
{
    [Theory]
    [InlineData("/api/auth/login")]
    [InlineData("/api/auth/refresh")]
    [InlineData("/api/auth/logout")]
    [InlineData("/api/auth/change-password")]
    public async Task TokenEndpointsAreUnavailable(string path)
    {
        var context = Context("POST", path, "https://domusmind.test");
        await Boundary().InvokeAsync(context, new FakeAntiforgery(true));
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
    }

    [Theory]
    [InlineData("https://other.test", true)]
    [InlineData("https://domusmind.test", false)]
    [InlineData("", true)]
    public async Task MutationRejectsInvalidOriginOrToken(string origin, bool validToken)
    {
        var context = Context("POST", "/api/families", origin);
        await Boundary().InvokeAsync(context, new FakeAntiforgery(validToken));
        Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
    }

    [Fact]
    public async Task ValidSameOriginMutationReachesController()
    {
        var context = Context("POST", "/api/families", "https://domusmind.test");
        await Boundary().InvokeAsync(context, new FakeAntiforgery(true));
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
    }

    [Fact]
    public async Task ReadDoesNotRequireRequestToken()
    {
        var context = Context("GET", "/api/families", "");
        await Boundary().InvokeAsync(context, new FakeAntiforgery(false));
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
    }

    private static BffRequestBoundary Boundary() => new(context =>
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        return Task.CompletedTask;
    });

    private static DefaultHttpContext Context(string method, string path, string origin)
    {
        var context = new DefaultHttpContext();
        context.Request.Method = method;
        context.Request.Path = path;
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("domusmind.test");
        context.Request.Headers.Origin = origin;
        context.Response.Body = new MemoryStream();
        return context;
    }

    private sealed class FakeAntiforgery(bool valid) : IAntiforgery
    {
        public AntiforgeryTokenSet GetAndStoreTokens(HttpContext context) => throw new NotSupportedException();
        public AntiforgeryTokenSet GetTokens(HttpContext context) => throw new NotSupportedException();
        public Task<bool> IsRequestValidAsync(HttpContext context) => Task.FromResult(valid);
        public void SetCookieTokenAndHeader(HttpContext context) => throw new NotSupportedException();
        public Task ValidateRequestAsync(HttpContext context) => throw new NotSupportedException();
    }
}
