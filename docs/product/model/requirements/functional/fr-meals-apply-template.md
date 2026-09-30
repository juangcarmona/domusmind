---
id: "FR-MEALS-APPLY-TEMPLATE"
type: "functional-requirement"
title: "Plan a week from a template"
status: "draft"
derived-from:
  - "UC-MEALS-APPLY-TEMPLATE"
  - "BR-MEALS-TEMPLATE-SNAPSHOT"
  - "BR-MEALS-ONE-PLAN-PER-WEEK"
verification:
  - scenario-ref: "SB-MEALS-APPLY-TEMPLATE-EMPTY-WEEK"
  - scenario-ref: "SB-MEALS-APPLY-TEMPLATE-DELETED-RECIPE"
  - scenario-ref: "SB-MEALS-APPLY-TEMPLATE-EXISTING-WEEK"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-PLAN"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Apply Weekly Template); src/backend/DomusMind.Domain/MealPlanning/Entities/MealPlan.cs (CreateFromTemplate); interview: product owner decision Q-0037 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household create a Draft plan for a target week pre-filled from a template as a snapshot, with uncovered slots Unplanned and the applied template recorded; it MUST fail without creating a plan when a referenced recipe no longer exists, and MUST return the existing plan when the week already has one (observed: openspec/specs/meal-planning/spec.md, Apply Weekly Template).

## Rationale

One-action reuse of a known pattern. Medium: the legacy surface still shows it disabled. Weekly templates are current product at medium confidence, flagged for the Meal Planning refinement change (decided: Q-0037).
