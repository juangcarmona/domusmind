---
id: "FR-MEALS-SLOT-ASSIGNMENT"
type: "functional-requirement"
title: "Assign or clear meal slots"
status: "draft"
derived-from:
  - "UC-MEALS-UPDATE-SLOT"
  - "BR-MEALS-LOCKED-SLOT"
  - "BR-MEALS-SLOT-SOURCE-CONSISTENCY"
  - "BR-MEALS-PLAN-LIFECYCLE"
verification:
  - scenario-ref: "SB-MEALS-ASSIGN-RECIPE-TO-SLOT"
  - scenario-ref: "SB-MEALS-CLEAR-SLOT"
  - scenario-ref: "SB-MEALS-LOCKED-SLOT-REJECTED"
  - scenario-ref: "SB-MEALS-UNLOCK-AND-CHANGE-SLOT"
  - scenario-ref: "SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-MEAL-SOURCE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Meal Slot Assignment); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (UpdateSlot)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household assign any slot of a Draft or Active plan to a recipe, free text, an external meal, leftovers or Unplanned, with optional notes and optional and locked flags, and MUST let it clear a slot back to Unplanned. It MUST reject changes to a locked slot unless the same change unlocks it, and MUST change slot content only, never the grid (observed: openspec/specs/meal-planning/spec.md, Meal Slot Assignment).

## Rationale

Not every meal needs a recipe; the grid must capture every realistic kind of meal (observed: surface Anti-Patterns "require a recipe for every slot").
