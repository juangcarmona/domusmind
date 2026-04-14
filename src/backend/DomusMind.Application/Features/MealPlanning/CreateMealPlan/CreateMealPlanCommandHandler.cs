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
    private readonly IEventLogWriter _eventLogWriter;

    public CreateMealPlanCommandHandler(
        IDomusMindDbContext dbContext,
        IEventLogWriter eventLogWriter)
    {
        _dbContext = dbContext;
        _eventLogWriter = eventLogWriter;
    }

    public async Task<CreateMealPlanResponse> Handle(
        CreateMealPlanCommand command,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var mealPlan = MealPlan.Create(
            MealPlanId.New(),
            FamilyId.From(command.FamilyId),
            command.WeekStart,
            now,
            now);

        _dbContext.Set<MealPlan>().Add(mealPlan);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventLogWriter.WriteAsync(mealPlan.DomainEvents, cancellationToken);
        mealPlan.ClearDomainEvents();

        return new CreateMealPlanResponse(
            mealPlan.Id.Value,
            mealPlan.FamilyId.Value,
            mealPlan.WeekStart,
            mealPlan.CreatedAtUtc,
            mealPlan.UpdatedAtUtc);
    }
}