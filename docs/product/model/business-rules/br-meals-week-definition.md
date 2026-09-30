---
id: "BR-MEALS-WEEK-DEFINITION"
type: "business-rule"
title: "A meal plan week starts on the household's first day of week and spans seven days"
status: "draft"
applies-to:
  - "UC-MEALS-CREATE-PLAN"
  - "BC-MEAL-PLANNING"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-HOUSEHOLD"
  - "TERM-HOUSEHOLD-SETTINGS"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Creation); docs/_legacy/04_contexts/meal-planning.md (MealPlan invariants); superseded on the week start: docs/_legacy/00_product/surfaces/meal-planning.md (Weekly Grid) and src/backend/DomusMind.Domain/MealPlanning/Enums/DayOfWeek.cs; interview: product owner decision Q-0035 (E-0158); interview: product owner decision Q-0056 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A meal plan's week start must align with the household's configured first day of week, and the week covers exactly seven days from that start (decided: Q-0035, Q-0056; observed: openspec/specs/meal-planning/spec.md, Meal Plan Creation). The first day of week is a household setting (see TERM-HOUSEHOLD-SETTINGS).

## Rationale

Households think in their own week, not a fixed calendar week (inferred).

## Examples

A household whose week starts on Sunday creates a plan starting on a Sunday and ending the following Saturday (inferred from the rule).

## Exceptions

None. The fixed Monday-to-Sunday week of the legacy surface and vocabulary is superseded (decided: Q-0035, Q-0056); the domain code's Monday-to-Sunday day enumeration names days and does not fix the week start (observed: src/backend/DomusMind.Domain/MealPlanning/Enums/DayOfWeek.cs).
