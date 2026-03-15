using DomusMind.Domain.Family;
using DomusMind.Domain.Responsibilities;
using DomusMind.Domain.Responsibilities.Events;
using DomusMind.Domain.Responsibilities.ValueObjects;
using FluentAssertions;

namespace DomusMind.Domain.Tests.Responsibilities;

public sealed class ResponsibilityDomainTests
{
    private static ResponsibilityDomain BuildDomain(string name = "Finances")
    {
        var id = ResponsibilityDomainId.New();
        var familyId = FamilyId.New();
        var areaName = ResponsibilityAreaName.Create(name);
        return Domain.Responsibilities.ResponsibilityDomain.Create(id, familyId, areaName, DateTime.UtcNow);
    }

    // ── ResponsibilityDomain.Create ────────────────────────────────────────────

    [Fact]
    public void Create_GivenValidInputs_SetsName()
    {
        var domain = BuildDomain("Health");

        domain.Name.Value.Should().Be("Health");
    }

    [Fact]
    public void Create_SetsCreatedAtUtc()
    {
        var before = DateTime.UtcNow;
        var domain = BuildDomain();
        var after = DateTime.UtcNow;

        domain.CreatedAtUtc.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Create_StartsWithNoPrimaryOwner()
    {
        var domain = BuildDomain();

        domain.PrimaryOwnerId.Should().BeNull();
    }

    [Fact]
    public void Create_StartsWithNoSecondaryOwners()
    {
        var domain = BuildDomain();

        domain.SecondaryOwnerIds.Should().BeEmpty();
    }

    [Fact]
    public void Create_EmitsOneResponsibilityDomainCreatedEvent()
    {
        var domain = BuildDomain();

        domain.DomainEvents.Should().HaveCount(1);
        domain.DomainEvents.Single().Should().BeOfType<ResponsibilityDomainCreated>();
    }

    [Fact]
    public void Create_Event_ContainsCorrectIds()
    {
        var id = ResponsibilityDomainId.New();
        var familyId = FamilyId.New();
        var domain = Domain.Responsibilities.ResponsibilityDomain.Create(
            id, familyId, ResponsibilityAreaName.Create("Pets"), DateTime.UtcNow);

        var evt = domain.DomainEvents.OfType<ResponsibilityDomainCreated>().Single();
        evt.ResponsibilityDomainId.Should().Be(id.Value);
        evt.FamilyId.Should().Be(familyId.Value);
    }

    // ── AssignPrimaryOwner ─────────────────────────────────────────────────────

    [Fact]
    public void AssignPrimaryOwner_SetsPrimaryOwner()
    {
        var domain = BuildDomain();
        var memberId = MemberId.New();

        domain.AssignPrimaryOwner(memberId);

        domain.PrimaryOwnerId.Should().Be(memberId);
    }

    [Fact]
    public void AssignPrimaryOwner_EmitsPrimaryOwnerAssignedEvent()
    {
        var domain = BuildDomain();
        domain.ClearDomainEvents();

        var memberId = MemberId.New();
        domain.AssignPrimaryOwner(memberId);

        domain.DomainEvents.Should().HaveCount(1);
        domain.DomainEvents.Single().Should().BeOfType<PrimaryOwnerAssigned>();
    }

    [Fact]
    public void AssignPrimaryOwner_Event_ContainsCorrectMemberId()
    {
        var domain = BuildDomain();
        domain.ClearDomainEvents();
        var memberId = MemberId.New();

        domain.AssignPrimaryOwner(memberId);

        var evt = domain.DomainEvents.OfType<PrimaryOwnerAssigned>().Single();
        evt.MemberId.Should().Be(memberId.Value);
        evt.ResponsibilityDomainId.Should().Be(domain.Id.Value);
    }

    [Fact]
    public void AssignPrimaryOwner_CanReassignToAnotherMember()
    {
        var domain = BuildDomain();
        var original = MemberId.New();
        var replacement = MemberId.New();
        domain.AssignPrimaryOwner(original);

        domain.AssignPrimaryOwner(replacement);

        domain.PrimaryOwnerId.Should().Be(replacement);
    }

    // ── AssignSecondaryOwner ───────────────────────────────────────────────────

    [Fact]
    public void AssignSecondaryOwner_AddsToSecondaryOwners()
    {
        var domain = BuildDomain();
        var memberId = MemberId.New();

        domain.AssignSecondaryOwner(memberId);

        domain.SecondaryOwnerIds.Should().Contain(memberId);
    }

    [Fact]
    public void AssignSecondaryOwner_EmitsSecondaryOwnerAssignedEvent()
    {
        var domain = BuildDomain();
        domain.ClearDomainEvents();
        var memberId = MemberId.New();

        domain.AssignSecondaryOwner(memberId);

        domain.DomainEvents.Should().HaveCount(1);
        domain.DomainEvents.Single().Should().BeOfType<SecondaryOwnerAssigned>();
    }

    [Fact]
    public void AssignSecondaryOwner_Event_ContainsCorrectMemberId()
    {
        var domain = BuildDomain();
        domain.ClearDomainEvents();
        var memberId = MemberId.New();

        domain.AssignSecondaryOwner(memberId);

        var evt = domain.DomainEvents.OfType<SecondaryOwnerAssigned>().Single();
        evt.MemberId.Should().Be(memberId.Value);
        evt.ResponsibilityDomainId.Should().Be(domain.Id.Value);
    }

    [Fact]
    public void AssignSecondaryOwner_MultipleDifferentMembers_AddsAll()
    {
        var domain = BuildDomain();
        var member1 = MemberId.New();
        var member2 = MemberId.New();

        domain.AssignSecondaryOwner(member1);
        domain.AssignSecondaryOwner(member2);

        domain.SecondaryOwnerIds.Should().HaveCount(2);
        domain.SecondaryOwnerIds.Should().Contain(member1);
        domain.SecondaryOwnerIds.Should().Contain(member2);
    }

    [Fact]
    public void AssignSecondaryOwner_DuplicateMemberId_ThrowsInvalidOperationException()
    {
        var domain = BuildDomain();
        var memberId = MemberId.New();
        domain.AssignSecondaryOwner(memberId);

        var act = () => domain.AssignSecondaryOwner(memberId);

        act.Should().Throw<InvalidOperationException>();
    }

    // ── TransferPrimaryOwner ───────────────────────────────────────────────────

    [Fact]
    public void TransferPrimaryOwner_SetsNewPrimaryOwner()
    {
        var domain = BuildDomain();
        var original = MemberId.New();
        var newOwner = MemberId.New();
        domain.AssignPrimaryOwner(original);

        domain.TransferPrimaryOwner(newOwner);

        domain.PrimaryOwnerId.Should().Be(newOwner);
    }

    [Fact]
    public void TransferPrimaryOwner_EmitsResponsibilityTransferredEvent()
    {
        var domain = BuildDomain();
        var original = MemberId.New();
        domain.AssignPrimaryOwner(original);
        domain.ClearDomainEvents();
        var newOwner = MemberId.New();

        domain.TransferPrimaryOwner(newOwner);

        domain.DomainEvents.Should().HaveCount(1);
        domain.DomainEvents.Single().Should().BeOfType<ResponsibilityTransferred>();
    }

    [Fact]
    public void TransferPrimaryOwner_Event_ContainsCorrectPreviousAndNewOwnerIds()
    {
        var domain = BuildDomain();
        var original = MemberId.New();
        domain.AssignPrimaryOwner(original);
        domain.ClearDomainEvents();
        var newOwner = MemberId.New();

        domain.TransferPrimaryOwner(newOwner);

        var evt = domain.DomainEvents.OfType<ResponsibilityTransferred>().Single();
        evt.PreviousPrimaryOwnerId.Should().Be(original.Value);
        evt.NewPrimaryOwnerId.Should().Be(newOwner.Value);
        evt.ResponsibilityDomainId.Should().Be(domain.Id.Value);
    }

    [Fact]
    public void TransferPrimaryOwner_WhenNoPreviousOwner_EventHasNullPreviousOwnerId()
    {
        var domain = BuildDomain();
        domain.ClearDomainEvents();
        var newOwner = MemberId.New();

        domain.TransferPrimaryOwner(newOwner);

        var evt = domain.DomainEvents.OfType<ResponsibilityTransferred>().Single();
        evt.PreviousPrimaryOwnerId.Should().BeNull();
    }
}
