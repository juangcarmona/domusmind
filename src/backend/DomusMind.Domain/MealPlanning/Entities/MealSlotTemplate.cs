using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.Enums;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class MealSlotTemplate : Entity<MealSlotTemplateId>
{
    public Enums.DayOfWeek DayOfWeek { get; private set; }
    
    public MealType MealType { get; private set; }
    
    public WeeklyTemplateId WeeklyTemplateId { get; private set; }
    
    public RecipeId? RecipeId { get; private set; }
    
    public string? Notes { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }

    private MealSlotTemplate() : base(default!)
    {
        WeeklyTemplateId = default!; // Initialize with default value to satisfy non-null requirement
    }

    private MealSlotTemplate(MealSlotTemplateId id, Enums.DayOfWeek dayOfWeek, MealType mealType, 
        WeeklyTemplateId weeklyTemplateId, RecipeId? recipeId, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        DayOfWeek = dayOfWeek;
        MealType = mealType;
        WeeklyTemplateId = weeklyTemplateId;
        RecipeId = recipeId;
        Notes = notes;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static MealSlotTemplate Create(MealSlotTemplateId id, Enums.DayOfWeek dayOfWeek, MealType mealType, 
        WeeklyTemplateId weeklyTemplateId, RecipeId? recipeId, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        return new MealSlotTemplate(id, dayOfWeek, mealType, weeklyTemplateId, recipeId, notes, createdAtUtc, updatedAtUtc);
    }

    public void Update(MealType? mealType = null, RecipeId? recipeId = null, string? notes = null)
    {
        if (mealType.HasValue)
            MealType = mealType.Value;
            
        if (recipeId != null)
            RecipeId = recipeId;
            
        if (notes != null)
            Notes = notes;
            
        UpdatedAtUtc = DateTime.UtcNow;
    }
}