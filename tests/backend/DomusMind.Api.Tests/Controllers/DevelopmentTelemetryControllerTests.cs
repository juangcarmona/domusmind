using DomusMind.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace DomusMind.Api.Tests.Controllers;

public sealed class DevelopmentTelemetryControllerTests
{
    [Fact]
    public async Task OutboundProbeCallsConfiguredLoopbackEndpointAndReturnsOnlyStatus()
    {
        var handler = new RecordingHandler();
        var clientFactory = new StubHttpClientFactory(handler);
        var controller = CreateController("Development", "http://localhost:61546/", clientFactory);

        var result = await controller.OutboundProbe(CancellationToken.None);

        var response = result.Should().BeOfType<OkObjectResult>().Subject;
        response.Value.Should().BeEquivalentTo(new { outboundStatusCode = 204 });
        handler.RequestUri.Should().Be(new Uri("http://localhost:61546/"));
    }

    [Theory]
    [InlineData("Production", "http://localhost:61546/")]
    [InlineData("Development", "https://example.com/")]
    [InlineData("Development", null)]
    public async Task OutboundProbeIsUnavailableOutsideDevelopmentOrWithoutLoopbackTarget(
        string environmentName,
        string? targetUrl)
    {
        var handler = new RecordingHandler();
        var controller = CreateController(
            environmentName,
            targetUrl,
            new StubHttpClientFactory(handler));

        var result = await controller.OutboundProbe(CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
        handler.RequestUri.Should().BeNull();
    }

    private static DevelopmentTelemetryController CreateController(
        string environmentName,
        string? targetUrl,
        IHttpClientFactory clientFactory)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DevelopmentTelemetry:OutboundProbeUrl"] = targetUrl,
            })
            .Build();

        return new DevelopmentTelemetryController(
            configuration,
            new TestHostEnvironment(environmentName),
            clientFactory);
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                Content = new StringContent("this response body is not returned"),
            });
        }
    }

    private sealed class StubHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
    }

    private sealed class TestHostEnvironment(string environmentName) : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = environmentName;
        public string ApplicationName { get; set; } = "DomusMind.Api.Tests";
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
