using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family.ValueObjects;

namespace DomusMind.Domain.Family;

public sealed class Member : Entity<MemberId>
{
    public MemberName Name { get; private set; }
    public MemberRole Role { get; private set; }
    public DateTime JoinedAtUtc { get; private set; }

    private Member(MemberId id, MemberName name, MemberRole role, DateTime joinedAtUtc)
        : base(id)
    {
        Name = name;
        Role = role;
        JoinedAtUtc = joinedAtUtc;
    }

    internal static Member Create(MemberId id, MemberName name, MemberRole role, DateTime joinedAtUtc)
        => new(id, name, role, joinedAtUtc);

#pragma warning disable CS8618
    // EF Core parameterless constructor
    private Member() : base(default) { }
#pragma warning restore CS8618
}
