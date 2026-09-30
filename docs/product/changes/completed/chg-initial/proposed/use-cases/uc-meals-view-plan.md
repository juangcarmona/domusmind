---
id: "UC-MEALS-VIEW-PLAN"
type: "use-case"
title: "See the week's meal plan"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-MEAL-TYPE-ORDER"
  - "BR-MEALS-FULL-SLOT-GRID"
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
  - "TERM-RECIPE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Meal Plan Viewing); docs/_legacy/00_product/surfaces/meal-planning.md (Default State, Core Layout, Header); docs/_legacy/04_contexts/meal-planning.md (Queries)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Answer at a glance what the household is eating this week and what is still missing.

## Trigger

A person opens the Meal Planning surface or navigates to another week.

## Preconditions

The person belongs to a household.

## Main Flow

1. The product opens on the current week for the household.
2. The product shows every slot of the plan, by day from the household's first day, then by meal type, including Unplanned slots.
3. For recipe slots the product shows the recipe name, servings and time information.
4. The person moves to the previous, current or next week with the week navigator.

## Alternative Flows

- 2a. The week has no plan: the product shows the week with compact actions to start from scratch, copy the previous week or apply a template, and no grid (observed: surface state 2).
- The current day is highlighted in the grid (observed: surface, Current Day).

## Failure Conditions

- A deleted recipe referenced by a Draft or Completed plan can no longer show its details (observed: recipe-management spec, Recipe Deletion).

## Postconditions

The person knows what is decided and what is still open for the week.
