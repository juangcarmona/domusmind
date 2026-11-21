using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

internal static class ObligationAccess
{
    internal static IQueryable<AssetObligation> Full(IDomusMindDbContext db) => db.Set<AssetObligation>()
        .Include(x => x.Occurrences).Include(x => x.Operations).AsSplitQuery();
    internal static async Task<AssetObligation> Find(IDomusMindDbContext db, Guid family, Guid asset, Guid id, CancellationToken ct)
        => await Full(db).SingleOrDefaultAsync(x => x.FamilyId == FamilyId.From(family)
            && x.AssetId == AssetId.From(asset) && x.Id == AssetObligationId.From(id), ct)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Obligation was not found in this household Asset.");
    internal static async Task<string> TimeZone(IDomusMindDbContext db, Guid family, CancellationToken ct)
        => await db.Set<Domain.Family.Family>().AsNoTracking().Where(x => x.Id == FamilyId.From(family))
            .Select(x => x.TimeZoneId).SingleOrDefaultAsync(ct)
            ?? throw new AssetsException(AssetsErrorCode.InvalidInput, "A manager must configure the household timezone in Household Settings first.");
    internal static ObligationRecurrence Recurrence(DefineObligationRequest request) => request.Recurrence switch
    {
        "OneTime" when request.IntervalMonths == 0 => ObligationRecurrence.OneTime(),
        "MonthlyLastDay" when request.IntervalMonths == 0 => ObligationRecurrence.MonthlyLastDay(),
        "CompletionRelative" => ObligationRecurrence.CompletionRelative(request.IntervalMonths),
        _ => throw new ArgumentException("Select OneTime, MonthlyLastDay or CompletionRelative with a valid interval.")
    };
    internal static ObligationDefinedResponse Defined(AssetObligation o) => new(o.Id.Value, o.AssetId.Value,
        o.DefinitionOperationId, o.Name, o.Occurrences.Single(x => !o.Operations.Any(p => p.ResultNextCycleId == x.Id)).Id.Value,
        o.InitialDueDate, o.Recurrence.Kind.ToString(), o.Recurrence.Months);
    internal static ObligationOperationResponse Map(ObligationOperation x) => new(x.Id, x.ObligationId.Value, x.CycleId.Value,
        x.Action.ToString(), x.ResultObligationState.ToString(), x.ResultOccurrenceState.ToString(), x.PreviousDueDate,
        x.ResultDueDate, x.ResultCompletedDate, x.ResultNextCycleId?.Value, x.ResultNextDueDate,
        // Match PostgreSQL microsecond precision in both the first response and durable replay.
        new DateTime(x.RecordedAtUtc.Ticks / 10 * 10, DateTimeKind.Utc));
    internal static AssetObligationResponse Map(AssetObligation o, string zone, DateTimeOffset now, bool archived)
    {
        var date = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById(zone)).DateTime);
        return new(o.Id.Value, o.AssetId.Value, o.Name, o.State.ToString(), o.Recurrence.Kind.ToString(), o.Recurrence.Months,
            zone, o.DueState(date, archived), o.Occurrences.OrderBy(x => x.DueDate).ThenBy(x => x.Id.Value)
                .Select(x => new AssetOccurrenceResponse(x.Id.Value, x.State.ToString(), x.DueDate, x.CompletedDate)).ToArray(),
            o.Operations.OrderBy(x => x.RecordedAtUtc).ThenBy(x => x.Id).Select(Map).ToArray());
    }
}
