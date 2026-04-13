using DomusMind.Domain.Abstractions;
using DomusMind.Domain.Family;
using DomusMind.Domain.MealPlanning.ValueObjects;

namespace DomusMind.Domain.MealPlanning.Entities;

public sealed class ShoppingList : AggregateRoot<ShoppingListId>
{
    public FamilyId FamilyId { get; private set; }
    
    public string Name { get; private set; }
    
    public MealPlanId? GeneratedFromMealPlanId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    private readonly List<ShoppingListItem> _items = new();
    public IReadOnlyList<ShoppingListItem> Items => _items.AsReadOnly();

    // Parameterless constructor for EF Core
    private ShoppingList() : base(default!)
    {
        Name = string.Empty; // Initialize with default value to satisfy non-null requirement
    }

    public ShoppingList(ShoppingListId id, FamilyId familyId, string name, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        FamilyId = familyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void AddItem(ShoppingListItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(ShoppingListItem item)
    {
        _items.Remove(item);
    }

    public void SetGeneratedFromMealPlan(MealPlanId mealPlanId)
    {
        GeneratedFromMealPlanId = mealPlanId;
    }
}