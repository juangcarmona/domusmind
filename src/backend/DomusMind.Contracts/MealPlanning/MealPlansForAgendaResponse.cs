using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Contracts.MealPlanning;

public sealed record MealPlansForAgendaResponse(
    IReadOnlyList<MealPlanForAgenda> MealPlans);

public sealed record MealPlanForAgenda(
    Guid Id,
    Guid FamilyId,
    DateOnly WeekStart,
    DateTime CreatedAt);