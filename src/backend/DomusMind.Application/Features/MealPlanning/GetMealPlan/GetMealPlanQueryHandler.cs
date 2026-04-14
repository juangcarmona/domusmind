using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.GetMealPlan;

public sealed class GetMealPlanQueryHandler : IQueryHandler<GetMealPlanQuery, GetMealPlanResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public GetMealPlanQueryHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<GetMealPlanResponse> Handle(GetMealPlanQuery query, CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            query.RequestedByUserId, query.FamilyId, cancellationToken);

        if (!canAccess)
            throw new InvalidOperationException("Access to this family is denied.");

        var familyId = FamilyId.From(query.FamilyId);

        var plan = await _dbContext.Set<MealPlan>()
            .Include(mp => mp.MealSlots)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                mp => mp.FamilyId == familyId && mp.WeekStart == query.WeekStart,
                cancellationToken);

        if (plan is null)
            return new GetMealPlanResponse(null);

        // Resolve recipe names for all assigned slots in one query.
        var recipeIds = plan.MealSlots
            .Where(s => s.RecipeId.HasValue)
            .Select(s => s.RecipeId!.Value)
            .Distinct()
            .ToList();

        Dictionary<RecipeId, string> recipeNames = [];
        if (recipeIds.Count > 0)
        {
            recipeNames = await _dbContext.Set<Recipe>()
                .AsNoTracking()
                .Where(r => recipeIds.Contains(r.Id))
                .Select(r => new { r.Id, r.Name })
                .ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);
        }

        var slots = plan.MealSlots
            .Select(s => new MealSlotDetail(
                s.Id.Value,
                s.DayOfWeek.ToString(),
                s.MealType.ToString(),
                s.RecipeId?.Value,
                s.RecipeId.HasValue && recipeNames.TryGetValue(s.RecipeId.Value, out var name) ? name : null,
                s.Notes))
            .ToList();

        var detail = new MealPlanDetail(
            plan.Id.Value,
            plan.FamilyId.Value,
            plan.WeekStart.ToString("yyyy-MM-dd"),
            plan.CreatedAtUtc,
            slots);

        return new GetMealPlanResponse(detail);
    }
}
