using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.MealPlanning.CreateRecipe;
using DomusMind.Application.Features.MealPlanning.GetFamilyRecipes;
using DomusMind.Contracts.MealPlanning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/recipes")]
[Authorize]
public sealed class RecipesController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public RecipesController(ICurrentUser currentUser) => _currentUser = currentUser;

    /// <summary>Creates a new recipe for a family.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRecipeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateRecipe(
        [FromBody] CreateRecipeRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new CreateRecipeCommand(
                    request.RecipeId, request.FamilyId, request.Name,
                    request.Description, request.PrepTimeMinutes, request.CookTimeMinutes,
                    request.Servings, request.IsFavorite,
                    request.AllowedMealTypes, request.Tags,
                    request.Ingredients,
                    _currentUser.UserId!.Value),
                cancellationToken);
            return Created($"/api/recipes/{response.Id}", response);
        }
        catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Returns all recipes for a family.</summary>
    [HttpGet("family/{familyId:guid}")]
    [ProducesResponseType(typeof(GetFamilyRecipesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetFamilyRecipes(
        Guid familyId,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new GetFamilyRecipesQuery(familyId, _currentUser.UserId!.Value),
                cancellationToken);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex) { return StatusCode(403, new { error = ex.Message }); }
    }
}
