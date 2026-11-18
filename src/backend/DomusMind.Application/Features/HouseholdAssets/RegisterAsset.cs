using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record RegisterAssetCommand(Guid FamilyId, Guid RequestedByUserId, AssetDetailsRequest Details) : ICommand<AssetResponse>;

public sealed class RegisterAssetCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<RegisterAssetCommand, AssetResponse>
{
    public async Task<AssetResponse> Handle(RegisterAssetCommand command, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, cancellationToken);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, cancellationToken);
        Asset asset;
        try
        {
            asset = Asset.Create(AssetId.New(), FamilyId.From(command.FamilyId), command.Details.Name, command.Details.Category);
        }
        catch (ArgumentException ex) { throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message); }
        await AssetAccess.ValidateReferences(db, command.FamilyId, asset.Id, command.Details, cancellationToken);
        AssetAccess.Apply(asset, command.Details);
        db.Set<Asset>().Add(asset);
        await events.WriteAsync([asset], cancellationToken);
        await mutation.CommitAsync(cancellationToken);
        asset.ClearDomainEvents();
        return AssetAccess.Map(asset);
    }
}
