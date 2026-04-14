using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class MealPlan : AggregateRoot<MealPlanId>
{
    public FamilyId FamilyId { get; private set; }
    
    public DateOnly WeekStart { get; private set; }
    
    public WeeklyTemplateId? TemplateId { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }
    
    private readonly List<MealSlot> _mealSlots = new();
    public IReadOnlyList<MealSlot> MealSlots => _mealSlots.AsReadOnly();

    // Parameterless constructor for EF Core
    private MealPlan() : base(default!)
    {
    }

    private MealPlan(MealPlanId id, FamilyId familyId, DateOnly weekStart, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        FamilyId = familyId;
        WeekStart = weekStart;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
        
        // Initialize meal slots for all meal types across each day of the week
        var mealTypes = new[] { MealType.Breakfast, MealType.Lunch, MealType.Dinner, MealType.Snack };
        for (int i = 0; i < 7; i++)
        {
            var day = (Enums.DayOfWeek)(i % 7);
            foreach (var mealType in mealTypes)
            {
                _mealSlots.Add(MealSlot.Create(
                    MealSlotId.New(),
                    day,
                    mealType,
                    id,
                    null,
                    null,
                    createdAtUtc,
                    updatedAtUtc
                ));
            }
        }
    }

    public static MealPlan Create(MealPlanId id, FamilyId familyId, DateOnly weekStart, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        var mealPlan = new MealPlan(id, familyId, weekStart, createdAtUtc, updatedAtUtc);
        mealPlan.RaiseDomainEvent(new MealPlanCreated(Guid.NewGuid(), id.Value, familyId.Value, weekStart, createdAtUtc));
        return mealPlan;
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