using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.HouseholdAssets;
using DomusMind.Contracts.HouseholdAssets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/families/{familyId:guid}/assets")]
public sealed class HouseholdAssetsController(ICurrentUser currentUser, ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Register(Guid familyId, AssetDetailsRequest request, CancellationToken ct)
        => Execute(async () =>
        {
            var result = await commands.Dispatch(new RegisterAssetCommand(familyId, currentUser.UserId!.Value, request), ct);
            return CreatedAtAction(nameof(Get), new { familyId, id = result.Id }, result);
        });

    [HttpGet]
    public Task<IActionResult> List(Guid familyId, [FromQuery] bool? archived, CancellationToken ct)
        => Execute(async () => Ok(await queries.Dispatch(new GetAssetsQuery(familyId, currentUser.UserId!.Value, archived), ct)));

    [HttpGet("categories")]
    public Task<IActionResult> Categories(Guid familyId, CancellationToken ct)
        => Execute(async () => Ok(await queries.Dispatch(new GetAssetCategoriesQuery(familyId, currentUser.UserId!.Value), ct)));

    [HttpGet("{id:guid}")]
    public Task<IActionResult> Get(Guid familyId, Guid id, CancellationToken ct)
        => Execute(async () => Ok(await queries.Dispatch(new GetAssetQuery(familyId, id, currentUser.UserId!.Value), ct)));

    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update(Guid familyId, Guid id, AssetDetailsRequest request, CancellationToken ct)
        => Execute(async () => Ok(await commands.Dispatch(new UpdateAssetCommand(familyId, id, currentUser.UserId!.Value, request), ct)));

    /// <summary>Ordinary removal preserves the asset by archiving it.</summary>
    [HttpDelete("{id:guid}")]
    [HttpPost("{id:guid}/archive")]
    public Task<IActionResult> Archive(Guid familyId, Guid id, CancellationToken ct)
        => Execute(async () => Ok(await commands.Dispatch(new ArchiveAssetCommand(familyId, id, currentUser.UserId!.Value), ct)));

    [HttpPost("{id:guid}/restore")]
    public Task<IActionResult> Restore(Guid familyId, Guid id, CancellationToken ct)
        => Execute(async () => Ok(await commands.Dispatch(new RestoreAssetCommand(familyId, id, currentUser.UserId!.Value), ct)));

    private async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId is null) return Unauthorized();
        try { return await action(); }
        catch (AssetsException ex) { return StatusCode(AssetHttpErrors.Status(ex.Code), new { error = ex.Message }); }
    }
}

internal static class AssetHttpErrors
{
    internal static int Status(AssetsErrorCode code) => code switch
    {
        AssetsErrorCode.AccessDenied => StatusCodes.Status403Forbidden,
        AssetsErrorCode.NotFound => StatusCodes.Status404NotFound,
        AssetsErrorCode.InvalidInput => StatusCodes.Status400BadRequest,
        AssetsErrorCode.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}
