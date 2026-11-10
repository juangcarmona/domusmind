using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DomusMind.Api.Auth;

/// <summary>Single-process ticket store used only by the development BFF.</summary>
public sealed class DevelopmentBffTicketStore : ITicketStore
{
    private readonly ConcurrentDictionary<string, TicketEntry> _tickets = new(StringComparer.Ordinal);
    private int _storedTicketCount;

    public Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        if (Interlocked.Increment(ref _storedTicketCount) % 64 == 0)
            RemoveExpiredTickets();

        var key = Guid.NewGuid().ToString("N");
        _tickets[key] = new TicketEntry(ticket, GetExpiration(ticket));
        return Task.FromResult(key);
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        if (_tickets.TryGetValue(key, out var existing))
            _tickets.TryUpdate(key, new TicketEntry(ticket, GetExpiration(ticket)), existing);
        return Task.CompletedTask;
    }

    public Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        if (!_tickets.TryGetValue(key, out var entry))
            return Task.FromResult<AuthenticationTicket?>(null);

        if (entry.ExpiresAtUtc <= DateTimeOffset.UtcNow)
        {
            _tickets.TryRemove(key, out _);
            return Task.FromResult<AuthenticationTicket?>(null);
        }

        return Task.FromResult<AuthenticationTicket?>(entry.Ticket);
    }

    public Task RemoveAsync(string key)
    {
        _tickets.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    private static DateTimeOffset GetExpiration(AuthenticationTicket ticket) =>
        ticket.Properties.ExpiresUtc ?? DateTimeOffset.UtcNow.AddMinutes(30);

    private void RemoveExpiredTickets()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var (key, entry) in _tickets)
        {
            if (entry.ExpiresAtUtc <= now)
                _tickets.TryRemove(key, out _);
        }
    }

    private sealed record TicketEntry(AuthenticationTicket Ticket, DateTimeOffset ExpiresAtUtc);
}
