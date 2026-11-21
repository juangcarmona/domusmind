namespace DomusMind.Contracts.HouseholdAssets;

public sealed record DefineObligationRequest(Guid OperationId, string Name, DateOnly DueDate,
    string Recurrence, int IntervalMonths = 0);
public sealed record ObligationDefinedResponse(Guid Id, Guid AssetId, Guid OperationId, string Name,
    Guid InitialCycleId, DateOnly InitialDueDate, string Recurrence, int IntervalMonths);
public sealed record ActOnObligationRequest(Guid OperationId, Guid CycleId, string Action, DateOnly? Date = null);
public sealed record ObligationOperationResponse(Guid OperationId, Guid ObligationId, Guid CycleId, string Action,
    string ObligationState, string OccurrenceState, DateOnly PreviousDueDate, DateOnly DueDate,
    DateOnly? CompletedDate, Guid? NextCycleId, DateOnly? NextDueDate, DateTime RecordedAtUtc);
public sealed record AssetOccurrenceResponse(Guid Id, string State, DateOnly DueDate, DateOnly? CompletedDate);
public sealed record AssetObligationResponse(Guid Id, Guid AssetId, string Name, string State,
    string Recurrence, int IntervalMonths, string TimeZoneId, string? DueState,
    IReadOnlyList<AssetOccurrenceResponse> Occurrences, IReadOnlyList<ObligationOperationResponse> History);
