---
id: "SB-RECIPES-RENAME-CONFLICT"
type: "structured-behaviour"
title: "Renaming a recipe to another recipe's name is rejected"
status: "draft"
illustrates:
  - "UC-RECIPES-UPDATE-RECIPE"
  - "BR-RECIPES-NAME-UNIQUE"
given:
  - "a recipe named \"Pasta Bolognese\" exists in the household's library"
  - "a person renames a different recipe to \"Pasta Bolognese\""
when: "the person saves the change"
then:
  - "the change is rejected"
  - "neither recipe is changed"
uses-terms:
  - "TERM-RECIPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (scenario: Household attempts to rename a recipe to a conflicting name); openspec/changes/recipe-management (delivered); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/UpdateRecipe; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Uniqueness holds on rename, not only on creation.

## Boundaries

None.
