---
id: "UC-RECIPES-BROWSE-LIBRARY"
type: "use-case"
title: "Browse the household recipe library"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-RECIPES-ALLOWED-MEAL-TYPES"
uses-terms:
  - "TERM-RECIPE"
  - "TERM-MEAL-TYPE"
provenance:
  source: "openspec/changes/recipe-management/specs/recipe-management/spec.md (Recipe Library Surface); openspec/changes/recipe-management/specs/meal-planning/spec.md (Recipe Library); openspec/changes/recipe-management/proposal.md; openspec/changes/recipe-management/design.md (Decisions 3 and 5); src/backend/DomusMind.Api/Controllers/RecipesController.cs; src/backend/DomusMind.Application/Features/MealPlanning/GetFamilyRecipes; interview: product owner decision Q-0039 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Find a recipe to plan, view or maintain.

## Trigger

A person opens the recipe library, or opens the recipe picker for a slot.

## Preconditions

The household has recipes.

## Main Flow

1. The person opens the recipe library surface, independent of the weekly plan.
2. The product lists all recipes ordered by name, each with cook time, servings, tag count and ingredient count.
3. The person filters by name and selects a recipe to view (UC-RECIPES-VIEW-RECIPE).

## Alternative Flows

- 1a. From a slot's recipe picker, the product lists only recipes compatible with that slot's meal type.

## Failure Conditions

None.

## Postconditions

The person has found the recipe. Delivered with the `recipe-management` change (decided: Q-0039); search beyond name filtering is out of scope (observed: openspec/changes/recipe-management/proposal.md).
