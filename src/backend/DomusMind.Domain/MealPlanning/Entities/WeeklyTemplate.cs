using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class WeeklyTemplate : AggregateRoot<WeeklyTemplateId>
{
    public FamilyId FamilyId { get; private set; }

    public string Name { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private readonly List<MealSlotTemplate> _mealSlotTemplates = new();
    public IReadOnlyList<MealSlotTemplate> MealSlotTemplates => _mealSlotTemplates.AsReadOnly();

    public WeeklyTemplate(WeeklyTemplateId id, FamilyId familyId, string name, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;

        // Initialize meal slot templates for each day of the week
        for (int i = 0; i < 7; i++)
        {
            var day = (Enums.DayOfWeek)i;
            _mealSlotTemplates.Add(new MealSlotTemplate(
                MealSlotTemplateId.New(),
                day,
                MealType.Dinner, // Default to dinner for now
                id,
                null,
                null
            ));
        }
    }

    public void AddMealSlotTemplate(MealSlotTemplate mealSlotTemplate)
    {
        _mealSlotTemplates.Add(mealSlotTemplate);
    }

    public void RemoveMealSlotTemplate(MealSlotTemplateId mealSlotTemplateId)
    {
        var mealSlotTemplate = _mealSlotTemplates.FirstOrDefault(mst => mst.Id == mealSlotTemplateId);
        if (mealSlotTemplate != null)
        {
            _mealSlotTemplates.Remove(mealSlotTemplate);
        }
    }

    public void UpdateSlotTemplate(MealSlotTemplateId mealSlotTemplateId, MealType? mealType = null,
        RecipeId? recipeId = null, string? notes = null)
    {
        var mealSlotTemplate = _mealSlotTemplates.FirstOrDefault(mst => mst.Id == mealSlotTemplateId);
        if (mealSlotTemplate != null)
        {
            mealSlotTemplate.Update(mealType, recipeId, notes);
        }
    }

    // Parameterless constructor for EF Core
    
#pragma warning disable CS8618
    private WeeklyTemplate() : base(default!)
    {
    }
#pragma warning restore CS8618
}