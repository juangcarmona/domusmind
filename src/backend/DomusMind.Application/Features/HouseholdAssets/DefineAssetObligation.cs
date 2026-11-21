using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record DefineAssetObligationCommand(Guid FamilyId, Guid AssetId, Guid RequestedByUserId,
    DefineObligationRequest Request) : ICommand<ObligationDefinedResponse>;
public sealed class DefineAssetObligationCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<DefineAssetObligationCommand, ObligationDefinedResponse>
{
    public async Task<ObligationDefinedResponse> Handle(DefineAssetObligationCommand command, CancellationToken ct)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, ct);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, ct);
        var asset = await db.Set<Asset>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == AssetId.From(command.AssetId)
            && x.FamilyId == FamilyId.From(command.FamilyId), ct)
            ?? throw new AssetsException(AssetsErrorCode.NotFound, "Asset was not found in this household.");
        await ObligationAccess.TimeZone(db, command.FamilyId, ct);
        var request = command.Request;
        ObligationRecurrence rule;
        try { rule = ObligationAccess.Recurrence(request); }
        catch (ArgumentException ex) { throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message); }
        var existing = await ObligationAccess.Full(db).SingleOrDefaultAsync(x => x.FamilyId == FamilyId.From(command.FamilyId)
            && x.DefinitionOperationId == request.OperationId, ct);
        if (existing is not null)
        {
            if (existing.AssetId != asset.Id || existing.Name != request.Name?.Trim() || existing.InitialDueDate != request.DueDate
                || existing.Recurrence.Kind != rule.Kind || existing.Recurrence.Months != rule.Months)
                throw new AssetsException(AssetsErrorCode.Conflict, "Operation identity already represents another definition.");
            return ObligationAccess.Defined(existing);
        }
        if (asset.IsArchived) throw new AssetsException(AssetsErrorCode.Conflict, "Archived assets are read-only.");
        AssetObligation obligation;
        try { obligation = AssetObligation.Create(AssetObligationId.New(), asset.FamilyId, asset.Id, request.OperationId, request.Name!, request.DueDate, rule); }
        catch (ArgumentException ex) { throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message); }
        db.Set<AssetObligation>().Add(obligation);
        await events.WriteAsync([obligation], ct);
        await mutation.CommitAsync(ct);
        obligation.ClearDomainEvents();
        return ObligationAccess.Defined(obligation);
    }
}
