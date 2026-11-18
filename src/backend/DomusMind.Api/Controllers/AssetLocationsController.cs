using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.HouseholdAssets;
using DomusMind.Contracts.HouseholdAssets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/families/{familyId:guid}/asset-locations")]
public sealed class AssetLocationsController(ICurrentUser currentUser, ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Register(Guid familyId, CreateAssetLocationRequest request, CancellationToken ct)
        => Execute(async () =>
        {
            var result = await commands.Dispatch(new RegisterAssetLocationCommand(familyId, currentUser.UserId!.Value, request), ct);
            return CreatedAtAction(nameof(List), new { familyId }, result);
        });

    [HttpGet]
    public Task<IActionResult> List(Guid familyId, CancellationToken ct)
        => Execute(async () => Ok(await queries.Dispatch(new GetAssetLocationsQuery(familyId, currentUser.UserId!.Value), ct)));

    private async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null) return Unauthorized();
        try { return await action(); }
        catch (AssetsException ex) { return StatusCode(AssetHttpErrors.Status(ex.Code), new { error = ex.Message }); }
    }
}
