namespace DomusMind.Contracts.Family;

public sealed record WeeklyGridEventItem(
    Guid EventId,
    string Title,
    DateTime StartTime,
    DateTime? EndTime,
    string Status);

public sealed record WeeklyGridTaskItem(
    Guid TaskId,
    string Title,
    DateTime? DueDate,
    string Status);

public sealed record WeeklyGridCell(
    DateTime Date,
    IReadOnlyCollection<WeeklyGridEventItem> Events,
    IReadOnlyCollection<WeeklyGridTaskItem> Tasks);

public sealed record WeeklyGridMember(
    Guid MemberId,
    string Name,
    string Role,
    IReadOnlyCollection<WeeklyGridCell> Cells);

public sealed record WeeklyGridRoutineItem(
    Guid RoutineId,
    string Name,
    string Cadence,
    string Status);

public sealed record WeeklyGridResponse(
    DateTime WeekStart,
    DateTime WeekEnd,
    IReadOnlyCollection<WeeklyGridMember> Members,
    IReadOnlyCollection<WeeklyGridRoutineItem> Routines);
