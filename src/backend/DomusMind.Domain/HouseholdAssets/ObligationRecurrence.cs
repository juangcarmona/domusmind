namespace DomusMind.Domain.HouseholdAssets;

/// <summary>Calculates the next household-local due date without generating future cycles.</summary>
public sealed class ObligationRecurrence
{
    private enum RecurrenceKind { OneTime, MonthlyLastDay, CompletionRelative }

    private readonly RecurrenceKind kind;
    private readonly int months;

    private ObligationRecurrence(RecurrenceKind kind, int months = 0)
    {
        this.kind = kind;
        this.months = months;
    }

    public static ObligationRecurrence OneTime() => new(RecurrenceKind.OneTime);
    public static ObligationRecurrence MonthlyLastDay() => new(RecurrenceKind.MonthlyLastDay);

    public static ObligationRecurrence CompletionRelative(int months)
    {
        if (months <= 0) throw new ArgumentOutOfRangeException(nameof(months));
        return new ObligationRecurrence(RecurrenceKind.CompletionRelative, months);
    }

    /// <summary>A skip advances from the pending date; completion-relative work advances from completion.</summary>
    public DateOnly? Next(DateOnly pendingDate, DateOnly? completionDate)
    {
        if (kind == RecurrenceKind.OneTime) return null;
        if (kind == RecurrenceKind.CompletionRelative)
            return (completionDate ?? pendingDate).AddMonths(months);

        var nextMonth = new DateOnly(pendingDate.Year, pendingDate.Month, 1).AddMonths(1);
        return new DateOnly(nextMonth.Year, nextMonth.Month, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
    }
}
