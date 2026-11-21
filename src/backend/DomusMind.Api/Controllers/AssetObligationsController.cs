using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.HouseholdAssets;
using DomusMind.Contracts.HouseholdAssets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/families/{familyId:guid}/assets/{assetId:guid}/obligations")]
public sealed class AssetObligationsController(ICurrentUser user, ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Define(Guid familyId, Guid assetId, DefineObligationRequest request, CancellationToken ct)
        => Execute(async () => StatusCode(StatusCodes.Status201Created,
            await commands.Dispatch(new DefineAssetObligationCommand(familyId, assetId, user.UserId!.Value, request), ct)));
    [HttpGet]
    public Task<IActionResult> List(Guid familyId, Guid assetId, CancellationToken ct)
        => Execute(async () => Ok(await queries.Dispatch(new GetAssetObligationsQuery(familyId, assetId, user.UserId!.Value), ct)));
    [HttpPost("{obligationId:guid}/operations")]
    public Task<IActionResult> Act(Guid familyId, Guid assetId, Guid obligationId, ActOnObligationRequest request, CancellationToken ct)
        => Execute(async () => Ok(await commands.Dispatch(new ActOnAssetObligationCommand(familyId, assetId, obligationId, user.UserId!.Value, request), ct)));
    private async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
    {
        if (!user.IsAuthenticated || user.UserId is null) return Unauthorized();
        try { return await action(); }
        catch (AssetsException ex) { return StatusCode(AssetHttpErrors.Status(ex.Code), new { error = ex.Message }); }
    }
}
