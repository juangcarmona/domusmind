using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;

namespace DomusMind.Domain.Tests.HouseholdAssets;

public sealed class AssetObligationTests
{
    private static readonly DateOnly Due = new(2025, 1, 31);
    private static AssetObligation Create(ObligationRecurrence? rule = null) => AssetObligation.Create(
        AssetObligationId.New(), FamilyId.New(), AssetId.New(), Guid.NewGuid(), "Service", Due,
        rule ?? ObligationRecurrence.MonthlyLastDay());
    private static ObligationOperation Act(AssetObligation o, ObligationAction action, DateOnly? date = null)
        => o.Apply(Guid.NewGuid(), o.Pending!.Id, action, date, false);

    [Fact]
    public void InitialPendingAndOverdueDoNotGenerateCycles()
    {
        var o = Create(); var id = o.Pending!.Id;
        Assert.Equal(ObligationState.Active, o.State);
        Assert.Equal("Overdue", o.DueState(Due.AddMonths(5), false));
        Assert.Single(o.Occurrences); Assert.Equal(id, o.Pending.Id);
    }

    [Fact]
    public void SuspendedRefusesActionsAndReactivationRetainsOverdueCycle()
    {
        var o = Create(); var id = o.Pending!.Id;
        Act(o, ObligationAction.Suspend);
        Assert.Equal(ObligationState.Suspended, o.State); Assert.Null(o.DueState(Due.AddDays(1), false));
        foreach (var action in new[] { ObligationAction.Complete, ObligationAction.Skip, ObligationAction.Reschedule, ObligationAction.Suspend })
            Assert.Throws<InvalidOperationException>(() => Act(o, action, action is ObligationAction.Skip or ObligationAction.Suspend ? null : Due));
        Assert.Single(o.Operations); Assert.Single(o.Occurrences);
        Act(o, ObligationAction.Reactivate);
        Assert.Equal(id, o.Pending!.Id); Assert.Equal(Due, o.Pending.DueDate);
        Assert.Equal("Overdue", o.DueState(Due.AddDays(10), false));
        Assert.Throws<InvalidOperationException>(() => Act(o, ObligationAction.Reactivate));
    }

    [Theory]
    [InlineData(false)] [InlineData(true)]
    public void CancelFromActiveOrSuspendedPreservesHistoryAndIsTerminal(bool suspended)
    {
        var o = Create(); Act(o, ObligationAction.Complete, Due);
        var previous = o.Occurrences.Single(x => x.State == OccurrenceState.Completed);
        if (suspended) Act(o, ObligationAction.Suspend);
        var pending = o.Pending!; Act(o, ObligationAction.Cancel);
        Assert.Equal(ObligationState.Cancelled, o.State); Assert.Null(o.Pending);
        Assert.Equal(OccurrenceState.Cancelled, pending.State); Assert.Null(pending.CompletedDate);
        Assert.Equal(OccurrenceState.Completed, previous.State);
        RefusesTerminalActions(o, pending.Id);
    }

    [Theory]
    [InlineData(ObligationAction.Complete)] [InlineData(ObligationAction.Skip)]
    public void OneTimeResolutionFinishesAndRefusesTerminalActions(ObligationAction action)
    {
        var o = Create(ObligationRecurrence.OneTime()); var id = o.Pending!.Id;
        Act(o, action, action == ObligationAction.Complete ? Due : null);
        Assert.Equal(ObligationState.Finished, o.State); Assert.Null(o.Pending); Assert.Single(o.Occurrences);
        Assert.Equal(action == ObligationAction.Complete ? OccurrenceState.Completed : OccurrenceState.Skipped, o.Occurrences.Single().State);
        RefusesTerminalActions(o, id);
    }

    [Fact]
    public void RecurringCompleteAndSkipEachAdvanceExactlyOnceAndOldCycleIsTerminal()
    {
        var o = Create(); var old = o.Pending!.Id;
        Act(o, ObligationAction.Complete, Due.AddDays(4));
        Assert.Equal(new DateOnly(2025, 2, 28), o.Pending!.DueDate);
        foreach (var action in Enum.GetValues<ObligationAction>())
            Assert.Throws<InvalidOperationException>(() => o.Apply(Guid.NewGuid(), old, action,
                action is ObligationAction.Complete or ObligationAction.Reschedule ? Due : null, false));
        Act(o, ObligationAction.Skip);
        Assert.Equal(new DateOnly(2025, 3, 31), o.Pending!.DueDate);
        Assert.Equal(3, o.Occurrences.Count); Assert.Single(o.Occurrences, x => x.State == OccurrenceState.Pending);
        Assert.Equal(ObligationState.Active, o.State);
    }

    [Fact]
    public void CompletionRelativeUsesActualCompletionButSkipUsesDueDate()
    {
        var o = Create(ObligationRecurrence.CompletionRelative(12));
        Act(o, ObligationAction.Complete, new DateOnly(2025, 3, 15));
        Assert.Equal(new DateOnly(2026, 3, 15), o.Pending!.DueDate);
        Act(o, ObligationAction.Skip); Assert.Equal(new DateOnly(2027, 3, 15), o.Pending!.DueDate);
    }

    [Fact]
    public void RescheduleRetainsCycleRuleAndAuditableDates()
    {
        var o = Create(); var id = o.Pending!.Id;
        var result = Act(o, ObligationAction.Reschedule, new DateOnly(2025, 2, 5));
        Assert.Equal(id, o.Pending!.Id); Assert.Equal(Due, result.PreviousDueDate);
        Assert.Equal(new DateOnly(2025, 2, 5), result.ResultDueDate);
        Act(o, ObligationAction.Complete, new DateOnly(2025, 2, 5));
        Assert.Equal(new DateOnly(2025, 3, 31), o.Pending!.DueDate);
    }

    [Fact]
    public void RetryReturnsOriginalResultAcrossFurtherCyclesSuspensionAndArchive()
    {
        var o = Create(); var id = o.Pending!.Id; var op = Guid.NewGuid();
        var accepted = o.Apply(op, id, ObligationAction.Complete, Due, false);
        Act(o, ObligationAction.Skip); Act(o, ObligationAction.Suspend);
        var version = o.Version;
        Assert.Same(accepted, o.Apply(op, id, ObligationAction.Complete, Due, true));
        Assert.Equal(version, o.Version); Assert.Equal(3, o.Operations.Count);
        Assert.Throws<InvalidOperationException>(() => o.Apply(op, id, ObligationAction.Skip, null, false));
    }

    [Fact]
    public void ArchivedAssetRefusesEveryNewActionAndRestorationDoesNotReactivate()
    {
        var o = Create(); var id = o.Pending!.Id;
        foreach (var action in Enum.GetValues<ObligationAction>())
            Assert.Throws<InvalidOperationException>(() => o.Apply(Guid.NewGuid(), id, action,
                action is ObligationAction.Complete or ObligationAction.Reschedule ? Due : null, true));
        Assert.Empty(o.Operations); Assert.Equal(Due, o.Pending.DueDate);
        Assert.Null(o.DueState(Due.AddDays(1), true)); Assert.Equal("Overdue", o.DueState(Due.AddDays(1), false));
        Act(o, ObligationAction.Suspend); Assert.Null(o.DueState(Due.AddDays(1), false));
        Assert.Equal(ObligationState.Suspended, o.State);
    }

    [Fact]
    public void InvalidRequestsAndDateOverflowDoNotPartiallyMutate()
    {
        var o = Create(); var id = o.Pending!.Id;
        Assert.Throws<ArgumentException>(() => o.Apply(Guid.Empty, id, ObligationAction.Skip, null, false));
        Assert.Throws<ArgumentException>(() => Act(o, ObligationAction.Complete));
        Assert.Throws<ArgumentException>(() => Act(o, ObligationAction.Skip, Due));
        Assert.Throws<ArgumentException>(() => Act(o, (ObligationAction)999));
        Assert.Empty(o.Operations); Assert.Equal(0, o.Version);
        var edge = AssetObligation.Create(AssetObligationId.New(), FamilyId.New(), AssetId.New(), Guid.NewGuid(), "Edge",
            DateOnly.MaxValue, ObligationRecurrence.MonthlyLastDay());
        Assert.Throws<ArgumentOutOfRangeException>(() => Act(edge, ObligationAction.Skip));
        Assert.Equal(OccurrenceState.Pending, edge.Pending!.State); Assert.Empty(edge.Operations);
    }

    private static void RefusesTerminalActions(AssetObligation o, AssetOccurrenceId id)
    {
        var count = o.Operations.Count;
        foreach (var action in Enum.GetValues<ObligationAction>())
            Assert.Throws<InvalidOperationException>(() => o.Apply(Guid.NewGuid(), id, action,
                action is ObligationAction.Complete or ObligationAction.Reschedule ? Due : null, false));
        Assert.Equal(count, o.Operations.Count);
    }
}
