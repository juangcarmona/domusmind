---
id: "TERM-MEAL-TYPE"
type: "domain-term"
title: "Meal Type"
status: "draft"
defined-in: "BC-MEAL-PLANNING"
synonyms:
  - "MealType"
uses-terms:
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Creation); docs/_legacy/04_contexts/meal-planning.md (Meal Type Taxonomy); src/backend/DomusMind.Domain/MealPlanning/Enums/MealType.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

The ordered set of meal moments in a day: Breakfast, Mid-morning snack, Lunch, Afternoon snack, Dinner. The order is meaningful and views must respect it (observed: docs/_legacy/04_contexts/meal-planning.md, Meal Type Taxonomy; openspec/specs/meal-planning/spec.md, Meal Plan Creation).

## Distinguish From

- Meal Source (TERM-MEAL-SOURCE): where a meal comes from, not when it is eaten.
- Plan time of day: meal types are not clock times.

## Usage

Rows of the weekly grid; also the vocabulary of a recipe's allowed meal types.
