using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Calendar.Events;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.Calendar;

public sealed class CalendarSubscription : AggregateRoot<CalendarSubscriptionId>
{
    public Guid FamilyId { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public string Label { get; private set; }
    public Guid? MemberId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public DateTime? LastAccessedAtUtc { get; private set; }
    public int AccessCount { get; private set; }

    private CalendarSubscription(CalendarSubscriptionId id, Guid familyId, Guid ownerUserId, string label,
        Guid? memberId, string tokenHash, DateTime createdAtUtc)
        : base(id)
    {
        FamilyId = familyId;
        OwnerUserId = ownerUserId;
        Label = label;
        MemberId = memberId;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = createdAtUtc.AddMonths(12);
    }

    public static CalendarSubscription Create(Guid familyId, Guid ownerUserId, string label,
        Guid? memberId, string tokenHash, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(label) || label.Trim().Length > 100)
            throw new ArgumentException("Label must contain between 1 and 100 characters.", nameof(label));
        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("Token hash is required.", nameof(tokenHash));
        var subscription = new CalendarSubscription(CalendarSubscriptionId.New(), familyId, ownerUserId,
            label.Trim(), memberId, tokenHash, DateTime.SpecifyKind(nowUtc, DateTimeKind.Utc));
        subscription.RaiseDomainEvent(new CalendarSubscriptionCreated(Guid.NewGuid(), subscription.Id.Value,
            familyId, ownerUserId, subscription.CreatedAtUtc));
        return subscription;
    }

    public void Revoke(DateTime nowUtc)
    {
        if (RevokedAtUtc.HasValue) return;
        RevokedAtUtc = DateTime.SpecifyKind(nowUtc, DateTimeKind.Utc);
        RaiseDomainEvent(new CalendarSubscriptionRevoked(Guid.NewGuid(), Id.Value, RevokedAtUtc.Value));
    }

    public void RecordAccess(DateTime nowUtc)
    {
        if (RevokedAtUtc.HasValue || nowUtc >= ExpiresAtUtc)
            throw new InvalidOperationException("Inactive subscriptions cannot be accessed.");
        LastAccessedAtUtc = DateTime.SpecifyKind(nowUtc, DateTimeKind.Utc);
        AccessCount++;
        RaiseDomainEvent(new CalendarSubscriptionAccessed(Guid.NewGuid(), Id.Value, LastAccessedAtUtc.Value, AccessCount));
    }

#pragma warning disable CS8618
    private CalendarSubscription() : base(default) { }
#pragma warning restore CS8618
}
