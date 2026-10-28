using DomusMind.Api.Auth;
using Microsoft.AspNetCore.Authentication;

namespace DomusMind.Api.Tests.Auth;

public sealed class DevelopmentBffTicketStoreTests
{
    [Fact]
    public async Task StoreAndRetrieveAsync_RetrievesTicketUntilRemoved()
    {
        var store = new DevelopmentBffTicketStore();
        var ticket = Ticket(DateTimeOffset.UtcNow.AddMinutes(1));

        var key = await store.StoreAsync(ticket);

        Assert.NotEqual("", key);
        Assert.Same(ticket, await store.RetrieveAsync(key));

        await store.RemoveAsync(key);

        Assert.Null(await store.RetrieveAsync(key));
    }

    [Fact]
    public async Task RetrieveAsync_RemovesExpiredTicket()
    {
        var store = new DevelopmentBffTicketStore();
        var key = await store.StoreAsync(Ticket(DateTimeOffset.UtcNow.AddSeconds(-1)));

        Assert.Null(await store.RetrieveAsync(key));
        Assert.Null(await store.RetrieveAsync(key));
    }

    [Fact]
    public async Task RenewAsync_UpdatesStoredTicket()
    {
        var store = new DevelopmentBffTicketStore();
        var key = await store.StoreAsync(Ticket(DateTimeOffset.UtcNow.AddMinutes(1)));
        var renewed = Ticket(DateTimeOffset.UtcNow.AddMinutes(2));

        await store.RenewAsync(key, renewed);

        Assert.Same(renewed, await store.RetrieveAsync(key));
    }

    private static AuthenticationTicket Ticket(DateTimeOffset expiresAt) => new(
        new System.Security.Claims.ClaimsPrincipal(),
        new AuthenticationProperties { ExpiresUtc = expiresAt },
        "test");
}
