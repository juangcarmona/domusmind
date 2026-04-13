using DomusMind.Domain.Abstractions;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class Ingredient : Entity<IngredientId>
{
    public string Name { get; private set; }
    public RecipeId RecipeId { get; private set; }
    public decimal Quantity { get; private set; }
    public string Unit { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Ingredient() : base(default!)
    {
    }

    public Ingredient(IngredientId id, string name, RecipeId recipeId, decimal quantity, string unit, string? notes = null) : base(id)
    {
        Name = name;
        RecipeId = recipeId;
        Quantity = quantity;
        Unit = unit;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string? name = null, decimal? quantity = null, string? unit = null, string? notes = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
            
        if (quantity.HasValue)
            Quantity = quantity.Value;
            
        if (unit != null)
            Unit = unit;
            
        if (notes != null)
            Notes = notes;
    }
}