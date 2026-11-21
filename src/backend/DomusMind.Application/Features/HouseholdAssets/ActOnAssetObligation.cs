using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record ActOnAssetObligationCommand(Guid FamilyId, Guid AssetId, Guid ObligationId, Guid RequestedByUserId,
    ActOnObligationRequest Request) : ICommand<ObligationOperationResponse>;
public sealed class ActOnAssetObligationCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<ActOnAssetObligationCommand, ObligationOperationResponse>
{
    public async Task<ObligationOperationResponse> Handle(ActOnAssetObligationCommand command, CancellationToken ct)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, ct);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, ct);
        // A request-scoped context reads after acquiring the same lock used by Asset archive.
        var asset = await db.Set<Asset>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == AssetId.From(command.AssetId)
            && x.FamilyId == Domain.Family.FamilyId.From(command.FamilyId), ct)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Asset was not found in this household.");
        var obligation = await ObligationAccess.Find(db, command.FamilyId, command.AssetId, command.ObligationId, ct);
        var request = command.Request;
        if (!Enum.TryParse<ObligationAction>(request.Action, false, out var action) || !Enum.IsDefined(action) || action.ToString() != request.Action)
            throw new AssetsException(AssetsErrorCode.InvalidInput, "Select a declared obligation action.");
        var version = obligation.Version;
        ObligationOperation result;
        try { result = obligation.Apply(request.OperationId, AssetOccurrenceId.From(request.CycleId), action, request.Date, asset.IsArchived); }
        catch (ArgumentException ex) { throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message); }
        catch (InvalidOperationException ex) { throw new AssetsException(AssetsErrorCode.Conflict, ex.Message); }
        if (obligation.Version == version) return ObligationAccess.Map(result);
        try { await events.WriteAsync([obligation], ct); await mutation.CommitAsync(ct); }
        catch (DbUpdateConcurrencyException)
        { throw new AssetsException(AssetsErrorCode.Conflict, "Obligation changed. Reload it before applying a new operation."); }
        obligation.ClearDomainEvents();
        return ObligationAccess.Map(result);
    }
}
