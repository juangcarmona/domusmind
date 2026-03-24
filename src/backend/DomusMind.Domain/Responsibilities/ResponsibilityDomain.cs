using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities.Events;
using DomusMind.Domain.Responsibilities.ValueObjects;

namespace DomusMind.Domain.Responsibilities;

public sealed class ResponsibilityDomain : AggregateRoot<ResponsibilityDomainId>
{
    private readonly List<MemberId> _secondaryOwnerIds = [];

    public FamilyId FamilyId { get; private set; }
    public ResponsibilityAreaName Name { get; private set; }
    public MemberId? PrimaryOwnerId { get; private set; }
    public IReadOnlyCollection<MemberId> SecondaryOwnerIds => _secondaryOwnerIds.AsReadOnly();
    public DateTime CreatedAtUtc { get; private set; }

    private ResponsibilityDomain(
        ResponsibilityDomainId id,
        FamilyId familyId,
        ResponsibilityAreaName name,
        DateTime createdAtUtc)
        : base(id)
    {
        FamilyId = familyId;
        Name = name;
        CreatedAtUtc = createdAtUtc;
    }

    public static ResponsibilityDomain Create(
        ResponsibilityDomainId id,
        FamilyId familyId,
        ResponsibilityAreaName name,
        DateTime createdAtUtc)
    {
        var domain = new ResponsibilityDomain(id, familyId, name, createdAtUtc);
        domain.RaiseDomainEvent(new ResponsibilityDomainCreated(
            Guid.NewGuid(), id.Value, familyId.Value, createdAtUtc));
        return domain;
    }

    public void AssignPrimaryOwner(MemberId memberId)
    {
        PrimaryOwnerId = memberId;
        RaiseDomainEvent(new PrimaryOwnerAssigned(
            Guid.NewGuid(), Id.Value, memberId.Value, DateTime.UtcNow));
    }

    public void AssignSecondaryOwner(MemberId memberId)
    {
        if (_secondaryOwnerIds.Contains(memberId))
            throw new InvalidOperationException(
                $"Member '{memberId.Value}' is already a secondary owner of this responsibility domain.");

        _secondaryOwnerIds.Add(memberId);
        RaiseDomainEvent(new SecondaryOwnerAssigned(
            Guid.NewGuid(), Id.Value, memberId.Value, DateTime.UtcNow));
    }

    public void TransferPrimaryOwner(MemberId newOwnerId)
    {
        var previousOwnerId = PrimaryOwnerId;
        PrimaryOwnerId = newOwnerId;
        RaiseDomainEvent(new ResponsibilityTransferred(
            Guid.NewGuid(), Id.Value, previousOwnerId?.Value, newOwnerId.Value, DateTime.UtcNow));
    }

    public void Rename(ResponsibilityAreaName newName)
    {
        Name = newName;
    }

#pragma warning disable CS8618
    // EF Core parameterless constructor
    private ResponsibilityDomain() : base(default) { }
#pragma warning restore CS8618
}
