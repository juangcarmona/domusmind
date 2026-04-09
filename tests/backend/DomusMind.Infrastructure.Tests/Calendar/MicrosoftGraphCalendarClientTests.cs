using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DomusMind.Infrastructure.Integrations.Calendar.Microsoft;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace DomusMind.Infrastructure.Tests.Calendar;

public sealed class MicrosoftGraphCalendarClientTests
{
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    private static MicrosoftGraphCalendarClient BuildClient(FakeHttpHandler handler)
    {
        var factory = new FakeHttpClientFactory(handler);
        return new MicrosoftGraphCalendarClient(
            factory,
            NullLogger<MicrosoftGraphCalendarClient>.Instance);
    }

    /// <summary>
    /// Returns a JSON body representing a final delta page (no nextLink, has deltaLink).
    /// </summary>
    private static HttpResponseMessage FinalDeltaPage(string deltaLink = "https://graph.microsoft.com/v1.0/me/calendars/cal-123/calendarView/delta?$deltatoken=abc123")
    {
        var payload = new
        {
            value = Array.Empty<object>(),
            @odata_deltaLink = deltaLink
        };

        // Build the JSON manually to ensure the odata key is correct.
        var json = $$"""{"value":[],"@odata.deltaLink":"{{deltaLink}}"}""";
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
    }

    // -----------------------------------------------------------------------
    // Tests: Initial sync URL structure
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetInitialEventsAsync_IncludesStartDateTimeAndEndDateTime_InUrl()
    {
        var handler = new FakeHttpHandler(_ => FinalDeltaPage());
        var client = BuildClient(handler);

        var windowStart = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var windowEnd = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        await foreach (var _ in client.GetInitialEventsAsync("tok", "cal-123", windowStart, windowEnd))
        {
            // consume first page only
            break;
        }

        handler.LastRequestUri.Should().NotBeNull();
        var query = handler.LastRequestUri!.Query;

        // Query string should include startDateTime and endDateTime.
        query.Should().Contain("startDateTime=", "Graph calendarView/delta requires startDateTime");
        query.Should().Contain("endDateTime=", "Graph calendarView/delta requires endDateTime");

        // Values should reflect the ISO 8601 UTC format.
        var unescaped = Uri.UnescapeDataString(query);
        unescaped.Should().Contain("2026-01-01T00:00:00", "windowStart date should appear in URL");
        unescaped.Should().Contain("2026-04-01T00:00:00", "windowEnd date should appear in URL");

        // Path should reference calendarView/delta.
        handler.LastRequestUri.AbsolutePath
            .Should().EndWith("calendarView/delta");
    }

    [Fact]
    public async Task GetInitialEventsAsync_UrlContainsCalendarId_Escaped()
    {
        var handler = new FakeHttpHandler(_ => FinalDeltaPage());
        var client = BuildClient(handler);

        var windowStart = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var windowEnd = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        await foreach (var _ in client.GetInitialEventsAsync("tok", "my calendar id", windowStart, windowEnd))
            break;

        // Calendar ID with spaces should be percent-encoded in the path.
        handler.LastRequestUri!.AbsolutePath.Should().Contain("my%20calendar%20id");
    }

    // -----------------------------------------------------------------------
    // Tests: Delta link treated as opaque
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetDeltaEventsAsync_WhenDeltaLinkIsFullUrl_UsesItVerbatim()
    {
        const string storedDeltaLink =
            "https://graph.microsoft.com/v1.0/me/calendars/cal-123/calendarView/delta?$deltatoken=very-opaque-token-abc";

        var handler = new FakeHttpHandler(_ => FinalDeltaPage(storedDeltaLink));
        var client = BuildClient(handler);

        await foreach (var _ in client.GetDeltaEventsAsync("tok", "cal-123", storedDeltaLink))
            break;

        handler.LastRequestUri.Should().NotBeNull();
        // The exact stored URL — including its token — must be used verbatim.
        handler.LastRequestUri!.AbsoluteUri.Should().Be(storedDeltaLink);
    }

    [Fact]
    public async Task GetDeltaEventsAsync_WhenDeltaLinkIsFullUrl_DoesNotAppendParams()
    {
        // Verify that no extra query params (like startDateTime) are appended to delta link requests.
        const string storedDeltaLink =
            "https://graph.microsoft.com/v1.0/me/calendars/cal-123/calendarView/delta?$deltatoken=abc";

        var handler = new FakeHttpHandler(_ => FinalDeltaPage(storedDeltaLink));
        var client = BuildClient(handler);

        await foreach (var _ in client.GetDeltaEventsAsync("tok", "cal-123", storedDeltaLink))
            break;

        var query = handler.LastRequestUri!.Query;
        query.Should().NotContain("startDateTime");
        query.Should().NotContain("endDateTime");
    }

    [Fact]
    public async Task GetInitialEventsAsync_DoesNotInclude_SelectOrTop_InUrl()
    {
        var handler = new FakeHttpHandler(_ => FinalDeltaPage());
        var client = BuildClient(handler);

        var windowStart = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var windowEnd = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        await foreach (var _ in client.GetInitialEventsAsync("tok", "cal-123", windowStart, windowEnd))
            break;

        var query = handler.LastRequestUri!.Query;
        query.Should().NotContain("$select", "calendarView/delta does not support $select");
        query.Should().NotContain("$top", "calendarView/delta does not support $top");
    }

    [Fact]
    public async Task GetInitialEventsAsync_SendsPreferHeader_WithMaxPageSize()
    {
        var handler = new FakeHttpHandler(_ => FinalDeltaPage());
        var client = BuildClient(handler);

        var windowStart = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var windowEnd = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        await foreach (var _ in client.GetInitialEventsAsync("tok", "cal-123", windowStart, windowEnd))
            break;

        handler.LastRequest!.Headers.TryGetValues("Prefer", out var prefer).Should().BeTrue();
        prefer!.Should().ContainSingle().Which.Should().Be("odata.maxpagesize=50");
    }

    // -----------------------------------------------------------------------
    // Tests: 4xx error handling
    // -----------------------------------------------------------------------

    [Fact]
    public async Task GetInitialEventsAsync_WhenGraphReturns400_ThrowsHttpRequestException()
    {
        var handler = new FakeHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("""{"error":{"code":"BadRequest","message":"startDateTime is required."}}""",
                System.Text.Encoding.UTF8, "application/json")
        });
        var client = BuildClient(handler);

        var windowStart = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var windowEnd = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        Func<Task> act = async () =>
        {
            await foreach (var _ in client.GetInitialEventsAsync("tok", "cal-123", windowStart, windowEnd))
            {
                // should throw before yielding any page
            }
        };

        await act.Should().ThrowAsync<HttpRequestException>(
            "a 400 response from Graph must surface as an exception, not silently abort");
    }

    [Fact]
    public async Task GetDeltaEventsAsync_WhenGraphReturns401_ThrowsHttpRequestException()
    {
        var handler = new FakeHttpHandler(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("""{"error":{"code":"InvalidAuthenticationToken"}}""",
                System.Text.Encoding.UTF8, "application/json")
        });
        var client = BuildClient(handler);

        Func<Task> act = async () =>
        {
            await foreach (var _ in client.GetDeltaEventsAsync("tok", "cal-123",
                "https://graph.microsoft.com/v1.0/me/calendars/cal-123/calendarView/delta?$deltatoken=abc"))
            {
            }
        };

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    // -----------------------------------------------------------------------
    // Fake infrastructure
    // -----------------------------------------------------------------------

    internal sealed class FakeHttpHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _respond;

        public Uri? LastRequestUri { get; private set; }
        public HttpRequestMessage? LastRequest { get; private set; }

        public FakeHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> respond)
            => _respond = respond;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri;
            LastRequest = request;
            return Task.FromResult(_respond(request));
        }
    }

    private sealed class FakeHttpClientFactory : System.Net.Http.IHttpClientFactory
    {
        private readonly HttpMessageHandler _handler;

        public FakeHttpClientFactory(HttpMessageHandler handler) => _handler = handler;

        public HttpClient CreateClient(string name) => new(_handler);
    }
}
