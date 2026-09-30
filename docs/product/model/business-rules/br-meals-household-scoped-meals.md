---
id: "BR-MEALS-HOUSEHOLD-SCOPED-MEALS"
type: "business-rule"
title: "Meals are planned for the household, not per person"
status: "draft"
applies-to:
  - "BC-MEAL-PLANNING"
  - "UC-MEALS-UPDATE-SLOT"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Purpose); docs/_legacy/00_product/surfaces/meal-planning.md (Role, Anti-Patterns); docs/_legacy/04_contexts/meal-planning.md (Context Scope Limits); interview: product owner decision Q-0040 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

Meal plans, recipes and templates belong to the household; the product does not assign meals to individual people or track per-person dietary preferences (observed: openspec/specs/meal-planning/spec.md, Purpose; docs/_legacy/00_product/surfaces/meal-planning.md, Anti-Patterns "introduce per-member meal assignment"; docs/_legacy/04_contexts/meal-planning.md, Context Scope Limits).

## Rationale

Meal Planning coordinates what the household eats together with minimal effort (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Role).

## Examples

A family plans one dinner for Thursday rather than one dinner per person.

## Exceptions

None.
