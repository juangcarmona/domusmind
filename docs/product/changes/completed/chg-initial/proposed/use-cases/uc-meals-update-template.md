---
id: "UC-MEALS-UPDATE-TEMPLATE"
type: "use-case"
title: "Change a weekly meal template"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-TEMPLATE-NAME-UNIQUE"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Weekly Templates: 'Templates may be updated after creation'); docs/_legacy/04_contexts/meal-planning.md (Commands: UpdateWeeklyTemplate; Domain Events: WeeklyTemplateUpdated)"
  confidence: "low"
  recovered-from: "documentation"
---

## Goal

Keep a saved template in line with how the household eats now.

## Trigger

A person edits an existing template.

## Preconditions

The template exists in the household's template library.

## Main Flow

1. The person opens the template.
2. The person adds or changes slot assignments.
3. The product saves the template.

## Alternative Flows

None stated.

## Failure Conditions

- The new name clashes with another template: the change is rejected (inferred from BR-MEALS-TEMPLATE-NAME-UNIQUE).

## Postconditions

The template reflects the change; plans already created from it are unaffected (BR-MEALS-TEMPLATE-SNAPSHOT). Stated in one sentence without scenarios and absent from the domain code (observed: src/backend/DomusMind.Domain/MealPlanning/Entities/WeeklyTemplate.cs has no update behaviour), so confidence low.
