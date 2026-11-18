using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.HouseholdAssets;
using DomusMind.Domain.Family;
using DomusMind.Domain.HouseholdAssets;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.HouseholdAssets;

public sealed record RegisterAssetLocationCommand(Guid FamilyId, Guid RequestedByUserId, CreateAssetLocationRequest Details) : ICommand<AssetLocationResponse>;

public sealed class RegisterAssetLocationCommandHandler(IDomusMindDbContext db, IFamilyAuthorizationService authorization,
    IAssetMutationLock mutationLock, IEventLogWriter events) : ICommandHandler<RegisterAssetLocationCommand, AssetLocationResponse>
{
    public async Task<AssetLocationResponse> Handle(RegisterAssetLocationCommand command, CancellationToken cancellationToken)
    {
        await AssetAccess.Authorize(authorization, command.RequestedByUserId, command.FamilyId, cancellationToken);
        await using var mutation = await mutationLock.BeginAsync(command.FamilyId, cancellationToken);
        AssetLocation location;
        try { location = AssetLocation.Create(AssetLocationId.New(), FamilyId.From(command.FamilyId), command.Details.Name, command.Details.Address); }
        catch (ArgumentException ex) { throw new AssetsException(AssetsErrorCode.InvalidInput, ex.Message); }
        db.Set<AssetLocation>().Add(location);
        await events.WriteAsync([location], cancellationToken);
        await mutation.CommitAsync(cancellationToken);
        location.ClearDomainEvents();
        return new(location.Id.Value, location.FamilyId.Value, location.Name, location.Address);
    }
}
