---
id: "FR-MEALS-WEEK-GRID-SURFACE"
type: "functional-requirement"
title: "Weekly meal planning surface"
status: "draft"
derived-from:
  - "UC-MEALS-VIEW-PLAN"
  - "UC-MEALS-UPDATE-SLOT"
verification:
  - scenario: "Opening Meal Planning shows the current week for the household with the full grid, or compact start actions when the week has no plan."
  - scenario: "Selecting a grid cell opens an inspector for that slot with meal source, notes, flags and clear and copy actions."
  - scenario: "Quick actions on a cell assign a recipe, free text, external or leftovers, copy from the previous day, or clear, without a modal."
  - scenario: "When copying the previous week finds no plan, an inline notice reads that no meal plan was found for the previous week and every other action stays available."
  - scenario: "When copying or applying a template for a week that already has a plan, the existing plan is shown with an inline notice that the week already has a meal plan."
uses-terms:
  - "TERM-MEAL-PLAN"
  - "TERM-MEAL-SLOT"
provenance:
  source: "docs/_legacy/00_product/surfaces/meal-planning.md (Default State, Core Layout, Interaction, Header, Surface State Model, UX Rules)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product MUST provide a weekly-first Meal Planning surface: a dense grid of days by meal types with the current day highlighted, a week navigator (previous, current, next), header actions for applying a template, copying the previous week and generating a shopping list, inline quick actions per cell, and an inspector as the only place for deep editing. Empty states MUST be actionable, actions in progress MUST not block the rest of the page, and recoverable failures MUST appear as compact inline notices (observed: docs/_legacy/00_product/surfaces/meal-planning.md). On mobile the grid is stacked day by day with a bottom sheet for detail (observed: docs/_legacy/00_product/surfaces/meal-planning.md, Shell).

## Rationale

Reuse must be faster than creation and a full week plannable in minutes (observed: surface, Success Criteria). Medium confidence: legacy surface prose, not an openspec requirement.
