using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.MealPlanning.CreateMealPlan;
using DomusMind.Application.Features.MealPlanning.CreateRecipe;
using DomusMind.Application.Features.MealPlanning.AssignMealToSlot;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/meal-plans")]
[Authorize]
public sealed class MealPlansController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public MealPlansController(ICurrentUser currentUser) => _currentUser = currentUser;

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
                    request.Id,
                    request.FamilyId,
                    request.WeekStart,
                    request.CreatedAt,
                    request.UpdatedAt),
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
                    request.Id,
                    request.FamilyId,
                    request.Name,
                    request.Description,
                    request.PrepTimeMinutes,
                    request.CookTimeMinutes,
                    request.Servings,
                    request.Instructions,
                    request.Notes,
                    request.CreatedAt,
                    request.UpdatedAt),
                cancellationToken);
            return Created($"/api/meal-plans/recipes/{response.Id}", response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    /// <summary>Assigns a meal to a meal slot.</summary>
    [HttpPut("{mealSlotId:guid}/meal")]
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
                    new MealSlotId(mealSlotId),
                    request.MealType,
                    request.RecipeId,
                    request.Notes),
                cancellationToken);
            return Ok(response);
        }
        catch (Exception ex) { return MapException(ex); }
    }

    private IActionResult MapException(Exception ex)
    {
        // Exception mapping logic would go here
        // For now, returning a generic error
        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred." });
    }
}