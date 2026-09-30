---
id: "UC-MEALS-CREATE-TEMPLATE"
type: "use-case"
title: "Save a weekly meal template"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-TEMPLATE-NAME-UNIQUE"
  - "BR-MEALS-SLOT-SOURCE-CONSISTENCY"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-MEAL-SLOT"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Weekly Templates); docs/_legacy/04_contexts/meal-planning.md (Commands: CreateWeeklyTemplate); src/backend/DomusMind.Domain/MealPlanning/Entities/WeeklyTemplate.cs; interview: product owner decision Q-0037 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Capture a usual weekly meal pattern so it can be reused without planning from scratch.

## Trigger

A person wants to keep a recurring pattern, for example a standard school week.

## Preconditions

The person belongs to a household.

## Main Flow

1. The person names the template.
2. The person optionally defines slot assignments by day and meal type, with the same sources and flags as meal slots.
3. The product saves the template to the household's template library.

## Alternative Flows

- 2a. The template covers only some slots; the rest will be Unplanned when applied.

## Failure Conditions

- A template with the same name already exists: creation is rejected.

## Postconditions

The template is available to apply to future weeks. No template creation flow appears on the legacy surface, where applying templates is shown as "coming soon" (observed: docs/_legacy/00_product/surfaces/meal-planning.md, state 6); confidence medium. Weekly templates are current product at medium confidence, flagged for the Meal Planning refinement change (decided: Q-0037).
