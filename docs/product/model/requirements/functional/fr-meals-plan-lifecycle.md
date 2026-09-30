---
id: "FR-MEALS-PLAN-LIFECYCLE"
type: "functional-requirement"
title: "Meal plan lifecycle"
status: "draft"
derived-from:
  - "UC-MEALS-PROMOTE-PLAN"
  - "BR-MEALS-PLAN-LIFECYCLE"
verification:
  - scenario-ref: "SB-MEALS-PROMOTE-TO-ACTIVE"
  - scenario-ref: "SB-MEALS-COMPLETED-PLAN-REJECTS-CHANGE"
uses-terms:
  - "TERM-MEAL-PLAN-STATUS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Requirement: Meal Plan Lifecycle, Notes); interview: product owner decision Q-0032 (E-0158); interview: product owner decision Q-0033 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST move meal plans through Draft, Active and Completed; a plan MUST be promoted to Active before it is treated as the household's working plan, and a Completed plan MUST refuse every change (observed: openspec/specs/meal-planning/spec.md, Meal Plan Lifecycle). Activation MUST be an explicit household action (decided: Q-0033). Draft and Active plans MUST both accept slot changes (decided: Q-0032). Undecided, deferred (Q-0033): what completes a plan is left to the Meal Planning refinement change.

## Rationale

Distinguishes a week being prepared from the week being lived and from history. Confidence medium: no promotion behaviour exists in the domain code today.
