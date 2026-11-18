using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record UpdateAssetCommand(Guid FamilyId, Guid AssetId, Guid RequestedByUserId, AssetDetailsRequest Details) : ICommand<AssetResponse>;

public sealed class UpdateAssetCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<UpdateAssetCommand, AssetResponse>
{
    public async Task<AssetResponse> Handle(UpdateAssetCommand command, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, cancellationToken);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, cancellationToken);
        var asset = await AssetAccess.Find(db, command.FamilyId, command.AssetId, cancellationToken);
        if (asset.IsArchived) throw new AssetsException(AssetsErrorCode.Conflict, "Archived assets are read-only.");
        await AssetAccess.ValidateReferences(db, command.FamilyId, asset.Id, command.Details, cancellationToken);
        AssetAccess.Apply(asset, command.Details);
        try
        {
            await events.WriteAsync([asset], cancellationToken);
            await mutation.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new AssetsException(AssetsErrorCode.Conflict, "The asset changed. Reload it before editing.");
        }
        asset.ClearDomainEvents();
        return AssetAccess.Map(asset);
    }
}
