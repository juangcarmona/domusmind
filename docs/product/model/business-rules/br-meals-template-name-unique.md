---
id: "BR-MEALS-TEMPLATE-NAME-UNIQUE"
type: "business-rule"
title: "Weekly template names are unique within a household"
status: "draft"
applies-to:
  - "UC-MEALS-CREATE-TEMPLATE"
  - "UC-MEALS-UPDATE-TEMPLATE"
uses-terms:
  - "TERM-WEEKLY-TEMPLATE"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Weekly Templates); docs/_legacy/04_contexts/meal-planning.md (WeeklyTemplate invariants)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A household cannot have two weekly templates with the same name, and a template defines at most one slot per day and meal type (observed: openspec/specs/meal-planning/spec.md, Weekly Templates; src/backend/DomusMind.Domain/MealPlanning/Entities/WeeklyTemplate.cs).

## Rationale

Templates are chosen by name when applying them (inferred).

## Examples

Creating a second template named "Standard week" is rejected (observed: openspec/specs/meal-planning/spec.md, scenario "Household attempts to create a template with a duplicate name").

## Exceptions

None.
