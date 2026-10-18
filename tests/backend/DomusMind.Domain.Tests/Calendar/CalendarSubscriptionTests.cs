using DomusMind.Domain.Calendar;
using FluentAssertions;

namespace DomusMind.Domain.Tests.Calendar;

public sealed class CalendarSubscriptionTests
{
    [Fact]
    public void Create_SetsTwelveMonthExpiryAndDoesNotExposeToken()
    {
        var now = new DateTime(2026, 10, 18, 12, 0, 0, DateTimeKind.Utc);
        var subscription = CalendarSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), "Family", null, "hash", now);

        subscription.ExpiresAtUtc.Should().Be(now.AddMonths(12));
        subscription.TokenHash.Should().Be("hash");
        subscription.DomainEvents.Should().ContainSingle();
    }

    [Fact]
    public void RecordAccess_TracksLastReadAndCount()
    {
        var subscription = Create();
        var readAt = new DateTime(2026, 10, 19, 8, 0, 0, DateTimeKind.Utc);

        subscription.RecordAccess(readAt);
        subscription.RecordAccess(readAt.AddMinutes(1));

        subscription.LastAccessedAtUtc.Should().Be(readAt.AddMinutes(1));
        subscription.AccessCount.Should().Be(2);
    }

    [Fact]
    public void RecordAccess_AfterExpiry_IsRejected()
    {
        var subscription = Create();

        var act = () => subscription.RecordAccess(subscription.ExpiresAtUtc);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Revoke_IsIdempotentAndMakesRevocationVisible()
    {
        var subscription = Create();
        var revokedAt = DateTime.UtcNow;

        subscription.Revoke(revokedAt);
        subscription.Revoke(revokedAt.AddMinutes(1));

        subscription.RevokedAtUtc.Should().Be(revokedAt);
    }

    private static CalendarSubscription Create()
        => CalendarSubscription.Create(Guid.NewGuid(), Guid.NewGuid(), "Calendar", null,
            "hash", DateTime.UtcNow);
}
