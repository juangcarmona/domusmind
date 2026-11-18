using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record RestoreAssetCommand(Guid FamilyId, Guid AssetId, Guid RequestedByUserId) : ICommand<AssetResponse>;

public sealed class RestoreAssetCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<RestoreAssetCommand, AssetResponse>
{
    public async Task<AssetResponse> Handle(RestoreAssetCommand command, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, cancellationToken);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, cancellationToken);
        var asset = await AssetAccess.Find(db, command.FamilyId, command.AssetId, cancellationToken);
        if (!asset.IsArchived) throw new AssetsException(AssetsErrorCode.Conflict, "Asset is already active.");
        asset.Restore();
        try
        {
            await events.WriteAsync([asset], cancellationToken);
            await mutation.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new AssetsException(AssetsErrorCode.Conflict, "The asset changed. Reload it before changing its lifecycle.");
        }
        asset.ClearDomainEvents();
        return AssetAccess.Map(asset);
    }
}
