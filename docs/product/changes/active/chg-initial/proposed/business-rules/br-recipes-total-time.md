---
id: "BR-RECIPES-TOTAL-TIME"
type: "business-rule"
title: "Total recipe time is the sum of preparation and cook time"
status: "draft"
applies-to:
  - "UC-RECIPES-CREATE-RECIPE"
  - "UC-RECIPES-UPDATE-RECIPE"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Recipe Library); src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs (TotalTimeMinutes); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetRecipeDetail; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When both preparation and cook times are given, total time is their sum; if either is missing, total time is unset (observed: openspec/specs/meal-planning/spec.md, Recipe Library; src/backend/DomusMind.Domain/MealPlanning/Entities/Recipe.cs).

## Rationale

Gives a quick "how long will this take" answer without asking for a third value (inferred).

## Examples

15 minutes preparation and 30 minutes cooking give 45 minutes total; with no cook time, no total is shown.

## Exceptions

None.
