using DomusMind.Domain.HouseholdAssets;
using FluentAssertions;

namespace DomusMind.Domain.Tests.HouseholdAssets;

public sealed class ObligationRecurrenceTests
{
    [Fact]
    public void OneTimeHasNoNextCycle()
    {
        ObligationRecurrence.OneTime().Next(new DateOnly(2025, 1, 31), new DateOnly(2025, 3, 15)).Should().BeNull();
    }

    [Fact]
    public void MonthlyLastDayRetainsAnchorAcrossFebruaryAndRescheduling()
    {
        var rule = ObligationRecurrence.MonthlyLastDay();
        rule.Next(new DateOnly(2025, 1, 31), null).Should().Be(new DateOnly(2025, 2, 28));
        rule.Next(new DateOnly(2024, 1, 31), null).Should().Be(new DateOnly(2024, 2, 29));
        rule.Next(new DateOnly(2025, 2, 5), null).Should().Be(new DateOnly(2025, 3, 31));
        rule.Next(new DateOnly(2025, 2, 28), null).Should().Be(new DateOnly(2025, 3, 31));
    }

    [Fact]
    public void CompletionRelativeUsesCompletionDateButSkipUsesDueDate()
    {
        var rule = ObligationRecurrence.CompletionRelative(12);
        var due = new DateOnly(2025, 1, 31);
        rule.Next(due, new DateOnly(2025, 3, 15)).Should().Be(new DateOnly(2026, 3, 15));
        rule.Next(due, null).Should().Be(new DateOnly(2026, 1, 31));
    }

    [Fact]
    public void CompletionRelativeRequiresPositiveInterval()
    {
        var invalid = () => ObligationRecurrence.CompletionRelative(0);
        invalid.Should().Throw<ArgumentOutOfRangeException>();
    }
}
