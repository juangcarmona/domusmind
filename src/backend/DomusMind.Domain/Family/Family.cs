using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family.Events;
using DomusMind.Domain.Family.ValueObjects;

namespace DomusMind.Domain.Family;

public sealed class Family : AggregateRoot<FamilyId>
{
    private readonly List<Member> _members = [];

    public FamilyName Name { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();

    private Family(FamilyId id, FamilyName name, DateTime createdAtUtc)
        : base(id)
    {
        Name = name;
        CreatedAtUtc = createdAtUtc;
    }

    public static Family Create(FamilyId id, FamilyName name, DateTime createdAtUtc)
    {
        var family = new Family(id, name, createdAtUtc);
        family.RaiseDomainEvent(new FamilyCreatedEvent(Guid.NewGuid(), id.Value, createdAtUtc));
        return family;
    }

    public Member AddMember(MemberId memberId, MemberName name, MemberRole role, DateTime addedAtUtc)
    {
        if (_members.Any(m => m.Id == memberId))
            throw new InvalidOperationException(
                $"A member with id '{memberId.Value}' already exists in this family.");

        var member = Member.Create(memberId, name, role, addedAtUtc);
        _members.Add(member);

        RaiseDomainEvent(new MemberAddedEvent(Guid.NewGuid(), Id.Value, memberId.Value, addedAtUtc));

        return member;
    }

#pragma warning disable CS8618
    // EF Core parameterless constructor
    private Family() : base(default) { }
#pragma warning restore CS8618
}
