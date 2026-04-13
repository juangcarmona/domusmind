using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Persistence;
using DomusMind.Application.Features.MealPlanning.CreateMealPlan;
using DomusMind.Contracts.MealPlanning;
using DomusMind.Domain.MealPlanning.Entities;
using DomusMind.Domain.MealPlanning.ValueObjects;
using DomusMind.Domain.Family;

namespace DomusMind.Application.Features.MealPlanning.CreateMealPlan;

public sealed class CreateMealPlanCommandHandler : ICommandHandler<CreateMealPlanCommand, CreateMealPlanResponse>
{
    private readonly IDomusMindDbContext _dbContext;

    public CreateMealPlanCommandHandler(IDomusMindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateMealPlanResponse> Handle(
        CreateMealPlanCommand command,
        CancellationToken cancellationToken)
    {
        var mealPlan = new MealPlan(
            command.Id,
            command.FamilyId,
            command.WeekStart,
            command.CreatedAt,
            command.UpdatedAt);

        _dbContext.Set<MealPlan>().Add(mealPlan);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateMealPlanResponse(
            mealPlan.Id,
            mealPlan.FamilyId,
            mealPlan.WeekStart,
            mealPlan.CreatedAt,
            mealPlan.UpdatedAt);
    }
}