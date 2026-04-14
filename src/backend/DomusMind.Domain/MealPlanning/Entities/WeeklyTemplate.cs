using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class WeeklyTemplate : AggregateRoot<WeeklyTemplateId>
{
    public FamilyId FamilyId { get; private set; }

    public string Name { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    private readonly List<MealSlotTemplate> _mealSlotTemplates = new();
    public IReadOnlyList<MealSlotTemplate> MealSlotTemplates => _mealSlotTemplates.AsReadOnly();

    private WeeklyTemplate(WeeklyTemplateId id, FamilyId familyId, string name, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;

        // Initialize meal slot templates for each day of the week
        for (int i = 0; i < 7; i++)
        {
            var day = (Enums.DayOfWeek)i;
            _mealSlotTemplates.Add(MealSlotTemplate.Create(
                MealSlotTemplateId.New(),
                day,
                MealType.Dinner, // Default to dinner for now
                id,
                null,
                null,
                createdAtUtc,
                updatedAtUtc
            ));
        }
    }

    public static WeeklyTemplate Create(WeeklyTemplateId id, FamilyId familyId, string name, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        var template = new WeeklyTemplate(id, familyId, name, createdAtUtc, updatedAtUtc);
        template.RaiseDomainEvent(new WeeklyTemplateCreated(Guid.NewGuid(), id.Value, familyId.Value, name, createdAtUtc));
        return template;
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