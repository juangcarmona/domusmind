using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Features.MealPlanning.GetMealPlansForAgenda;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusMind.Application.Features.MealPlanning.GetMealPlansForAgenda;

public sealed class GetMealPlansForAgendaQueryHandler
    : IQueryHandler<GetMealPlansForAgendaQuery, MealPlansForAgendaResponse>
{
    private readonly IDomusMindDbContext _dbContext;
    private readonly IFamilyAuthorizationService _authorizationService;

    public GetMealPlansForAgendaQueryHandler(
        IDomusMindDbContext dbContext,
        IFamilyAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
    }

    public async Task<MealPlansForAgendaResponse> Handle(
        GetMealPlansForAgendaQuery query,
        CancellationToken cancellationToken)
    {
        var canAccess = await _authorizationService.CanAccessFamilyAsync(
            query.RequestedByUserId, query.FamilyId, cancellationToken);
        if (!canAccess)
            throw new Exception("Access to this family is denied.");

        var familyId = FamilyId.From(query.FamilyId);

        // For demonstration purposes - this would query the meal plan data
        // In a real implementation, this would project from domain events or DB
        var mealPlans = await _dbContext.Set<MealPlan>()
            .AsNoTracking()
            .Where(mp => mp.FamilyId == familyId && mp.WeekStart == query.WeekStart)
            .Select(mp => new MealPlanForAgenda(
                mp.Id.Value,
                mp.FamilyId.Value,
                mp.WeekStart,
                mp.CreatedAt))
            .ToListAsync(cancellationToken);

        return new MealPlansForAgendaResponse(mealPlans);
    }
}