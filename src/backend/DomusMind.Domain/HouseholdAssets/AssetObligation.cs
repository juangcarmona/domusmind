using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;

namespace DomusMind.Domain.HouseholdAssets;

public readonly record struct AssetObligationId(Guid Value) : IAggregateIdentifier
{
    public static AssetObligationId New() => new(Guid.NewGuid());
    public static AssetObligationId From(Guid value) => new(value);
}
public readonly record struct AssetOccurrenceId(Guid Value)
{
    public static AssetOccurrenceId New() => new(Guid.NewGuid());
    public static AssetOccurrenceId From(Guid value) => new(value);
}
public enum ObligationState { Active, Suspended, Cancelled, Finished }
public enum OccurrenceState { Pending, Completed, Skipped, Cancelled }
public enum ObligationAction { Complete, Skip, Reschedule, Suspend, Reactivate, Cancel }

public sealed class AssetObligation : AggregateRoot<AssetObligationId>
{
    private readonly List<AssetOccurrence> _occurrences = [];
    private readonly List<ObligationOperation> _operations = [];
    public FamilyId FamilyId { get; private set; }
    public AssetId AssetId { get; private set; }
    public Guid DefinitionOperationId { get; private set; }
    public string Name { get; private set; } = "";
    public DateOnly InitialDueDate { get; private set; }
    public ObligationRecurrence Recurrence { get; private set; } = null!;
    public ObligationState State { get; private set; }
    public long Version { get; private set; }
    public IReadOnlyCollection<AssetOccurrence> Occurrences => _occurrences.AsReadOnly();
    public IReadOnlyCollection<ObligationOperation> Operations => _operations.AsReadOnly();
    public AssetOccurrence? Pending => _occurrences.SingleOrDefault(x => x.State == OccurrenceState.Pending);
    private AssetObligation(AssetObligationId id) : base(id) { }

    public static AssetObligation Create(AssetObligationId id, FamilyId familyId, AssetId assetId,
        Guid operationId, string name, DateOnly dueDate, ObligationRecurrence recurrence)
    {
        if (id.Value == Guid.Empty || familyId.Value == Guid.Empty || assetId.Value == Guid.Empty || operationId == Guid.Empty)
            throw new ArgumentException("Non-empty obligation, household, Asset and operation identities are required.");
        if (string.IsNullOrWhiteSpace(name) || dueDate == default)
            throw new ArgumentException("A name and due date are required.");
        ArgumentNullException.ThrowIfNull(recurrence);
        var result = new AssetObligation(id) { FamilyId = familyId, AssetId = assetId,
            DefinitionOperationId = operationId, Name = name.Trim(), InitialDueDate = dueDate, Recurrence = recurrence };
        result._occurrences.Add(new AssetOccurrence(AssetOccurrenceId.New(), id, dueDate));
        result.RaiseDomainEvent(new AssetObligationDefined(id.Value, assetId.Value, familyId.Value, DateTime.UtcNow));
        return result;
    }

    public string? DueState(DateOnly householdDate, bool assetArchived)
        => assetArchived || State != ObligationState.Active || Pending is not { } pending ? null
            : pending.DueDate < householdDate ? "Overdue" : pending.DueDate == householdDate ? "Due" : "Upcoming";

    public ObligationOperation Apply(Guid operationId, AssetOccurrenceId cycle, ObligationAction action,
        DateOnly? date, bool assetArchived)
    {
        if (operationId == Guid.Empty || cycle.Value == Guid.Empty || !Enum.IsDefined(action))
            throw new ArgumentException("A valid operation, cycle and action are required.");
        if ((action is ObligationAction.Complete or ObligationAction.Reschedule) != date.HasValue || date == default(DateOnly))
            throw new ArgumentException("Only completion and rescheduling require a non-empty local date.");
        var retry = _operations.SingleOrDefault(x => x.Id == operationId);
        if (retry is not null)
        {
            if (retry.CycleId != cycle || retry.Action != action || retry.RequestedDate != date)
                throw new InvalidOperationException("Operation identity already represents another request.");
            return retry;
        }
        if (assetArchived) throw new InvalidOperationException("Archived assets are read-only.");
        var pending = Pending;
        if (pending is null || pending.Id != cycle || State is ObligationState.Cancelled or ObligationState.Finished)
            throw new InvalidOperationException("This cycle is no longer actionable.");
        if (action == ObligationAction.Reactivate ? State != ObligationState.Suspended
            : action != ObligationAction.Cancel && State != ObligationState.Active)
            throw new InvalidOperationException("This action is not allowed in the obligation's current state.");

        // Calculate before mutating so an unrepresentable next date cannot leave a partial outcome.
        var nextDate = action is ObligationAction.Complete or ObligationAction.Skip
            ? Recurrence.Next(pending.DueDate, action == ObligationAction.Complete ? date : null) : null;
        var before = pending.DueDate;
        switch (action)
        {
            case ObligationAction.Complete:
                pending.Resolve(OccurrenceState.Completed, date);
                break;
            case ObligationAction.Skip:
                pending.Resolve(OccurrenceState.Skipped, null);
                break;
            case ObligationAction.Reschedule:
                pending.Reschedule(date!.Value);
                break;
            case ObligationAction.Suspend: State = ObligationState.Suspended; break;
            case ObligationAction.Reactivate: State = ObligationState.Active; break;
            case ObligationAction.Cancel:
                State = ObligationState.Cancelled;
                pending.Resolve(OccurrenceState.Cancelled, null);
                break;
        }
        AssetOccurrence? next = null;
        if (action is ObligationAction.Complete or ObligationAction.Skip)
        {
            if (nextDate is { } due) { next = new AssetOccurrence(AssetOccurrenceId.New(), Id, due); _occurrences.Add(next); }
            else State = ObligationState.Finished;
        }
        var result = new ObligationOperation(operationId, Id, cycle, action, date, before,
            State, pending.State, pending.DueDate, pending.CompletedDate, next?.Id, next?.DueDate, DateTime.UtcNow);
        _operations.Add(result);
        Version++;
        RaiseDomainEvent(new AssetObligationActedOn(Id.Value, FamilyId.Value, cycle.Value, operationId, action.ToString(), result.RecordedAtUtc));
        return result;
    }
}

public sealed class AssetOccurrence
{
    public AssetOccurrenceId Id { get; private set; }
    public AssetObligationId ObligationId { get; private set; }
    public DateOnly DueDate { get; private set; }
    public OccurrenceState State { get; private set; }
    public DateOnly? CompletedDate { get; private set; }
    private AssetOccurrence() { }
    internal AssetOccurrence(AssetOccurrenceId id, AssetObligationId obligationId, DateOnly dueDate)
        => (Id, ObligationId, DueDate) = (id, obligationId, dueDate);
    internal void Resolve(OccurrenceState state, DateOnly? completedDate)
        => (State, CompletedDate) = (state, completedDate);
    internal void Reschedule(DateOnly date) => DueDate = date;
}

// The immutable receipt is also the reschedule audit trail and original retry response.
public sealed record ObligationOperation(Guid Id, AssetObligationId ObligationId, AssetOccurrenceId CycleId,
    ObligationAction Action, DateOnly? RequestedDate, DateOnly PreviousDueDate, ObligationState ResultObligationState,
    OccurrenceState ResultOccurrenceState, DateOnly ResultDueDate, DateOnly? ResultCompletedDate,
    AssetOccurrenceId? ResultNextCycleId, DateOnly? ResultNextDueDate, DateTime RecordedAtUtc);
public sealed record AssetObligationDefined(Guid ObligationId, Guid AssetId, Guid FamilyId, DateTime OccurredAtUtc) : IDomainEvent;
public sealed record AssetObligationActedOn(Guid ObligationId, Guid FamilyId, Guid CycleId, Guid OperationId,
    string Action, DateTime OccurredAtUtc) : IDomainEvent;
