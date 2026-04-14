using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.MealPlanning.AssignMealToSlot;
using DomusMind.Application.Features.MealPlanning.CreateMealPlan;
using DomusMind.Application.Features.MealPlanning.CreateRecipe;
using DomusMind.Application.Features.MealPlanning.GetFamilyRecipes;
using DomusMind.Application.Features.MealPlanning.GetMealPlan;
using DomusMind.Contracts.MealPlanning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers.MealPlanning;

[ApiController]
[Route("api/meal-plans")]
[Authorize]
public sealed class MealPlansController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public MealPlansController(ICurrentUser currentUser) => _currentUser = currentUser;

    /// <summary>Returns the meal plan for a family week, or null when none exists.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetMealPlanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMealPlan(
        [FromQuery] Guid familyId,
        [FromQuery] DateOnly weekStart,
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new GetMealPlanQuery(familyId, weekStart, _currentUser.UserId!.Value),
                cancellationToken);
            return Ok(response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    /// <summary>Returns all recipes belonging to a family.</summary>
    [HttpGet("recipes")]
    [ProducesResponseType(typeof(GetFamilyRecipesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetFamilyRecipes(
        [FromQuery] Guid familyId,
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
        catch (Exception ex) { return MapException(ex); }
    }

    /// <summary>Creates a new meal plan for a family.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateMealPlanResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateMealPlan(
        [FromBody] CreateMealPlanRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new CreateMealPlanCommand(
                    request.FamilyId,
                    request.WeekStart,
                    _currentUser.UserId!.Value),
                cancellationToken);
            return Created($"/api/meal-plans/{response.Id}", response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    /// <summary>Creates a new recipe for a family.</summary>
    [HttpPost("recipes")]
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
                    request.FamilyId,
                    request.Name,
                    request.Description,
                    request.PrepTimeMinutes,
                    request.CookTimeMinutes,
                    request.Servings,
                    request.Instructions,
                    request.Notes,
                    _currentUser.UserId!.Value),
                cancellationToken);
            return Created($"/api/meal-plans/recipes/{response.Id}", response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    /// <summary>Assigns a meal to a meal slot.</summary>
    [HttpPatch("{mealSlotId:guid}/meal")]
    [ProducesResponseType(typeof(AssignMealToSlotResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AssignMealToSlot(
        Guid mealSlotId,
        [FromBody] AssignMealToSlotRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new AssignMealToSlotCommand(
                    mealSlotId,
                    request.MealType,
                    request.RecipeId,
                    request.Notes,
                    _currentUser.UserId!.Value),
                cancellationToken);
            return Ok(response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    private IActionResult MapException(Exception ex) =>
        StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
}
