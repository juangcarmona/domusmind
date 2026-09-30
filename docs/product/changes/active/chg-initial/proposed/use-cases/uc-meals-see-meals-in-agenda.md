---
id: "UC-MEALS-SEE-MEALS-IN-AGENDA"
type: "use-case"
title: "See planned meals in the Agenda"
status: "draft"
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-MEAL-PLANNING"
governed-by:
  - "BR-MEALS-AGENDA-PROJECTION"
uses-terms:
  - "TERM-MEAL-SLOT"
  - "TERM-AGENDA"
provenance:
  source: "openspec/specs/meal-planning/spec.md (Agenda Projection); docs/_legacy/04_contexts/meal-planning.md (Agenda Projection); docs/_legacy/00_product/surfaces/meal-planning.md (Relationship with Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See the day's meals together with the rest of the household's day.

## Trigger

A person views the Agenda for a day that has planned meals.

## Preconditions

A meal plan exists for the week containing that day.

## Main Flow

1. The person opens the Agenda for a day.
2. The product shows that day's projected meal slots as non-timed household entries, visually distinct from Plans and Tasks.

## Alternative Flows

- 2a. The person wants to change a meal: editing happens only in Meal Planning; the Agenda entry is read-only.

## Failure Conditions

None.

## Postconditions

The person sees the day's meals; nothing in Meal Planning changes.
