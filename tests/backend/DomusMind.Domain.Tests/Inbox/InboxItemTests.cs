using DomusMind.Domain.Family;
using DomusMind.Domain.Inbox;
using DomusMind.Domain.Inbox.Events;
using FluentAssertions;

namespace DomusMind.Domain.Tests.Inbox;

public sealed class InboxItemTests
{
    [Fact]
    public void Capture_PreservesOriginalNoteAndRaisesEvent()
    {
        var familyId = FamilyId.New();
        var item = InboxItem.Capture(InboxItemId.New(), familyId, Guid.NewGuid(), "  Pay camp fee  ", DateTime.UtcNow);

        item.RawText.Should().Be("Pay camp fee");
        item.FamilyId.Should().Be(familyId);
        item.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<InboxItemCaptured>();
    }

    [Fact]
    public void RejectProposal_ChangesOnlyThatProposal()
    {
        var item = BuildItem();
        var rejected = item.Proposals.First();

        item.Reject(rejected.Id);

        rejected.Status.Should().Be("Rejected");
        item.Proposals.Last().Status.Should().Be("Pending");
        item.DomainEvents.Should().Contain(x => x is InboxProposalChanged);
    }

    [Fact]
    public void AppliedProposal_StoresResultIdAndCannotBeAppliedTwice()
    {
        var item = BuildItem();
        var proposal = item.Proposals.First();
        var resultId = Guid.NewGuid();

        item.BeginApproval(proposal.Id, Guid.NewGuid());
        item.MarkApplied(proposal.Id, resultId);

        proposal.Status.Should().Be("Applied");
        proposal.ResultId.Should().Be(resultId);
        var act = () => item.MarkApplied(proposal.Id, Guid.NewGuid());
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ReminderProposal_StoresSelectedPlanAndOffsetAfterRefinement()
    {
        var item = InboxItem.Capture(InboxItemId.New(), FamilyId.New(), Guid.NewGuid(), "Remind about camp", DateTime.UtcNow);
        item.SetProposals([InboxProposal.Create("Reminder", "Remind before camp", null, null, null, null, null,
            null, null, "Camp Muir", 30)]);
        var proposal = item.Proposals.Single();
        var planId = Guid.NewGuid();

        item.Refine(proposal.Id, proposal.Title, proposal.Description, null, null, null, null, null, planId, 60);

        proposal.TargetPlanTitle.Should().Be("Camp Muir");
        proposal.TargetPlanId.Should().Be(planId);
        proposal.ReminderMinutesBefore.Should().Be(60);
    }

    [Fact]
    public void TimelineNoteProposal_IsAccepted()
    {
        var proposal = InboxProposal.Create("TimelineNote", "First day at home", null,
            "2030-06-23", null, null, null, null, null);

        proposal.Type.Should().Be("TimelineNote");
        proposal.Date.Should().Be("2030-06-23");
    }

    private static InboxItem BuildItem()
    {
        var item = InboxItem.Capture(InboxItemId.New(), FamilyId.New(), Guid.NewGuid(), "Judo camp", DateTime.UtcNow);
        item.ClearDomainEvents();
        item.SetProposals([
            InboxProposal.Create("Plan", "Judo camp", null, "2026-06-22", null, "2026-06-28", null, null, null),
            InboxProposal.Create("Task", "Pay camp", null, null, null, null, null, "2026-05-01", null)
        ]);
        item.ClearDomainEvents();
        return item;
    }
}
