using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class MealPlan : AggregateRoot<MealPlanId>
{
    public FamilyId FamilyId { get; private set; }
    
    public DateOnly WeekStart { get; private set; }
    
    public WeeklyTemplateId? TemplateId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<MealSlot> _mealSlots = new();
    public IReadOnlyList<MealSlot> MealSlots => _mealSlots.AsReadOnly();

    // Parameterless constructor for EF Core
    private MealPlan() : base(default!)
    {
    }

    public MealPlan(MealPlanId id, FamilyId familyId, DateOnly weekStart, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        FamilyId = familyId;
        WeekStart = weekStart;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        
        // Initialize meal slots for each day of the week
        for (int i = 0; i < 7; i++)
        {
            var day = (Enums.DayOfWeek)(i % 7);
            _mealSlots.Add(new MealSlot(
                MealSlotId.New(),
                day,
                MealType.Dinner, // Default to dinner for now
                id,
                null,
                null
            ));
        }
    }

    public void AddMealSlot(MealSlot mealSlot)
    {
        _mealSlots.Add(mealSlot);
    }

    public void RemoveMealSlot(MealSlotId mealSlotId)
    {
        var mealSlot = _mealSlots.FirstOrDefault(ms => ms.Id == mealSlotId);
        if (mealSlot != null)
        {
            _mealSlots.Remove(mealSlot);
        }
    }

    public void UpdateSlot(MealSlotId mealSlotId, MealType? mealType = null, RecipeId? recipeId = null, string? notes = null)
    {
        var mealSlot = _mealSlots.FirstOrDefault(ms => ms.Id == mealSlotId);
        if (mealSlot != null)
        {
            mealSlot.Update(mealType, recipeId, notes);
        }
    }

    public void SetTemplate(WeeklyTemplateId templateId)
    {
        TemplateId = templateId;
    }
}