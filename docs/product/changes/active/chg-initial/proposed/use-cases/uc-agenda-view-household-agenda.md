---
id: UC-AGENDA-VIEW-HOUSEHOLD-AGENDA
type: use-case
title: "See the household Agenda"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-AGENDA-PROJECTION-READ-ONLY"
  - "BR-AGENDA-ENTRY-PRIORITY-ORDER"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-OWNER-SCOPE-ONLY"
  - "BR-LISTS-EDIT-ONLY-IN-LISTS"
  - "BR-LISTS-PLAN-LINK-DOES-NOT-PROJECT"
  - "BR-AGENDA-LIST-ITEMS-HOUSEHOLD-ROW"
  - "BR-AGENDA-DEFAULT-ENTRY-STATE"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Scope: Household, Time Modes, Data, Item Priority Order); openspec/specs/calendar/spec.md (Household Timeline Projection); docs/_legacy/00_product/experience.md (Agenda)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

See what is happening in the household today, this week or this month, who has what, where load is concentrated and what is unassigned (observed: agenda.md, Purpose, Household scope).

## Trigger

The person opens the Agenda (the default landing surface) or switches to Household scope (observed: agenda.md, Entry Points).

## Preconditions

- The person belongs to a household.

## Main Flow

1. DomusMind shows Household scope, Day mode, today, as a board: a shared row with household plans, unassigned tasks and projected list items, then one row per person (observed: agenda.md).
2. Each row lists entries in priority order; a collapsed row shows at most two entries and summarizes the rest as "+N", expanding in place, one row at a time (observed: agenda.md).
3. Plans, tasks, routine occurrences and temporal list items for the window all appear; imported external entries never do (observed: calendar spec).

## Alternative Flows

- 1a. The person switches to Week: days as columns with plans, a routine lane and a task lane; overloaded days stand out (observed: agenda.md, Week).
- 1b. The person switches to Month: a grid with a count or titles per day and a marker for days with tasks or routines; today is distinguished (observed: agenda.md, Month).

## Failure Conditions

- Nothing is scheduled for the window: the board is shown empty (inferred).

## Postconditions

- The person has seen the household's temporal picture; no record was created or changed (observed: calendar spec).
