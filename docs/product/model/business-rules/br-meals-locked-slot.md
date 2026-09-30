---
id: "BR-MEALS-LOCKED-SLOT"
type: "business-rule"
title: "A locked slot changes only when unlocked in the same change"
status: "draft"
applies-to:
  - "UC-MEALS-UPDATE-SLOT"
uses-terms:
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Slot Assignment scenarios); docs/_legacy/04_contexts/meal-planning.md (MealSlot invariants); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A change to the content of a locked slot is rejected unless the same change unlocks it; when it does, the unlock and the content change apply together (observed: openspec/specs/meal-planning/spec.md, scenarios "Household attempts to mutate a locked slot" and "Household unlocks and mutates a locked slot"; src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs).

## Rationale

Locking protects stable routines (for example weekday breakfasts) from accidental change (observed: docs/_legacy/04_contexts/meal-planning.md, MealSlot isLocked).

## Examples

A locked Monday breakfast stays as it is when someone tries to replace it; changing it requires unlocking in the same edit.

## Exceptions

None.
