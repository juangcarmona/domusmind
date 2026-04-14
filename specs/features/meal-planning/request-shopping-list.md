# Spec - Request Shopping List

## Purpose

Derive a shopping list from the current meal plan's ingredient requirements.

This operation consolidates all ingredients from all assigned recipes in a meal plan, deduplicates by ingredient name, and creates a `List` of kind `shopping` in the Lists context.

## Context

- Module: Meal Planning
- Aggregate: `MealPlan` (source) → `List` (created in Lists context)
- Slice: `request-shopping-list`
- Command: `RequestShoppingList`
- Cross-context: triggers `List` creation in Lists context via domain event

## Inputs

Required:

- `mealPlanId`
- `familyId`

Optional:

- `shoppingListName` (defaults to e.g. "Shopping list - week of {weekStart}")

## Preconditions

- target meal plan must exist
- meal plan must belong to the requesting family
- meal plan must have at least one assigned recipe slot

## State Changes

### In Meal Planning

On success, the `MealPlan` aggregate records:

- `shoppingListId` reference to the created list (once confirmed)
- timestamp of last shopping list request

### In Shared Lists (via event reaction)

After `ShoppingListRequested` is emitted, the Shared Lists context creates:

- a new `SharedList` of kind `shopping`
- with name provided (or default)
- with one `SharedListItem` per consolidated ingredient
  - item name = ingredient name
  - quantity = consolidated quantity (if all units match; otherwise separate items)
  - checked = false

**Meal Planning does not own the shopping list after creation.**  
The list immediately becomes a first-class `SharedList` in the Shared Lists context.

## Invariants

- `shoppingListVersion` increments on every successful derivation request
- each derivation creates a new `List`; previous lists are not mutated
- consolidation deduplicates by ingredient name (case-insensitive) within the same unit; different units are kept as separate items
- the derived `List` belongs to the same family as the meal plan

## Events

Emit:

- `ShoppingListRequested` (from Meal Planning — carries ingredient consolidation payload)

React to (Lists context):

- `ListCreated` (emitted after list is created — Meal Planning updates `shoppingListId` reference)

## Success Result

Return:

- `mealPlanId`
- `shoppingListId`
- `shoppingListName`
- `itemCount`

## Failure Cases

- meal plan not found
- meal plan does not belong to family
- no assigned recipe slots in the plan (cannot derive ingredients)

## Notes

After the shopping list is created, household members interact with it entirely through the Lists surface — checking off items, adding manual extras, reordering.

Meal Planning does not receive feedback when list items are checked off.

This is an intentional boundary: purchase execution belongs to the Lists context.
