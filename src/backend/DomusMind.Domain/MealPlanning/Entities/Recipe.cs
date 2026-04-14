using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.Events;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class Recipe : AggregateRoot<RecipeId>
{
    public FamilyId FamilyId { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Description { get; private set; }
    
    public int? PrepTimeMinutes { get; private set; }
    
    public int? CookTimeMinutes { get; private set; }
    
    public int? Servings { get; private set; }
    
    public string? Instructions { get; private set; }
    
    public string? Notes { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }
    
    public DateTime UpdatedAtUtc { get; private set; }

    private readonly List<Ingredient> _ingredients = new();
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();

    // Parameterless constructor for EF Core
    private Recipe() : base(default!)
    {
        Name = string.Empty; // Initialize with default value to satisfy non-null requirement
    }

    private Recipe(RecipeId id, FamilyId familyId, string name, string? description, int? prepTimeMinutes, 
        int? cookTimeMinutes, int? servings, string? instructions, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        PrepTimeMinutes = prepTimeMinutes;
        CookTimeMinutes = cookTimeMinutes;
        Servings = servings;
        Instructions = instructions;
        Notes = notes;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public static Recipe Create(RecipeId id, FamilyId familyId, string name, string? description, int? prepTimeMinutes, 
        int? cookTimeMinutes, int? servings, string? instructions, string? notes, DateTime createdAtUtc, DateTime updatedAtUtc)
    {
        var recipe = new Recipe(id, familyId, name, description, prepTimeMinutes, cookTimeMinutes, servings, instructions, notes, createdAtUtc, updatedAtUtc);
        recipe.RaiseDomainEvent(new RecipeCreated(Guid.NewGuid(), id.Value, familyId.Value, name, createdAtUtc));
        return recipe;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(IngredientId ingredientId)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient != null)
        {
            _ingredients.Remove(ingredient);
        }
    }

    public void Update(string? name = null, string? description = null, int? prepTimeMinutes = null, 
        int? cookTimeMinutes = null, int? servings = null, string? instructions = null, string? notes = null)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name;
            
        if (description != null)
            Description = description;
            
        if (prepTimeMinutes.HasValue)
            PrepTimeMinutes = prepTimeMinutes.Value;
            
        if (cookTimeMinutes.HasValue)
            CookTimeMinutes = cookTimeMinutes.Value;
            
        if (servings.HasValue)
            Servings = servings.Value;
            
        if (instructions != null)
            Instructions = instructions;
            
        if (notes != null)
            Notes = notes;
            
        UpdatedAtUtc = DateTime.UtcNow;
    }
}