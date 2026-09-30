---
id: "UC-MEALS-UPDATE-SLOT"
type: "use-case"
title: "Assign or clear a meal slot"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-FULL-SLOT-GRID"
  - "BR-MEALS-PLAN-LIFECYCLE"
  - "BR-MEALS-LOCKED-SLOT"
  - "BR-MEALS-SLOT-SOURCE-CONSISTENCY"
  - "BR-RECIPES-ALLOWED-MEAL-TYPES"
  - "BR-MEALS-HOUSEHOLD-SCOPED-MEALS"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-SOURCE"
  - "TERM-RECIPE"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Slot Assignment); docs/_legacy/00_product/surfaces/meal-planning.md (Quick Actions, Inspector); docs/_legacy/04_contexts/meal-planning.md (Commands: UpdateMealSlot); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (UpdateSlot); interview: product owner decision Q-0038 (E-0158); interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Record what the household will eat at a given day and meal, or leave it open.

## Trigger

A person selects a cell of the weekly grid, or uses an inline quick action on it.

## Preconditions

A Draft or Active meal plan exists for the week.

## Main Flow

1. The person selects a slot (day and meal type).
2. The person chooses a meal source: a recipe from the household library, free text, eaten out (external), leftovers, or unplanned.
3. The person optionally adds notes and sets the optional or locked flags.
4. The product updates the slot content and keeps the grid structure unchanged.

## Alternative Flows

- 2a. Clear: the slot returns to Unplanned and any recipe or free text is removed.
- 2b. Choosing a recipe: the product offers only recipes compatible with the slot's meal type (BR-RECIPES-ALLOWED-MEAL-TYPES; decided: Q-0039). A recipe without ingredients can still be chosen (decided: Q-0038).
- 2c. Quick actions assign a recipe, free text, external or leftovers, copy from the previous day, or clear, without opening the inspector (observed: surface, Quick Actions).
- 3a. The slot is locked and the change unlocks it: the unlock and the content change apply together.

## Failure Conditions

- The slot is locked and the change does not unlock it: the change is rejected and the slot is unchanged.
- The plan is Completed: the change is rejected.
- Recipe chosen without a recipe, or free text left empty: the change is rejected.

## Postconditions

The slot shows the new content; the Agenda reflects it.
