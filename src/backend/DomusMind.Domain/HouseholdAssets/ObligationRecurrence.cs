namespace DomusMind.Domain.HouseholdAssets;

/// <summary>Calculates the next household-local due date without generating future cycles.</summary>
public sealed class ObligationRecurrence
{
    public enum RecurrenceKind { OneTime, MonthlyLastDay, CompletionRelative }

    public RecurrenceKind Kind { get; private set; }
    public int Months { get; private set; }

    private ObligationRecurrence(RecurrenceKind kind, int months = 0)
    {
        Kind = kind;
        Months = months;
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
        if (Kind == RecurrenceKind.OneTime) return null;
        if (Kind == RecurrenceKind.CompletionRelative)
            return (completionDate ?? pendingDate).AddMonths(Months);

        var nextMonth = new DateOnly(pendingDate.Year, pendingDate.Month, 1).AddMonths(1);
        return new DateOnly(nextMonth.Year, nextMonth.Month, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
    }
}
