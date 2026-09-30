---
id: SB-AGENDA-HOUSEHOLD-ALL-SOURCES
type: structured-behaviour
title: "The household timeline combines plans, tasks, routines and list items"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-HOUSEHOLD-AGENDA"
  - "BR-AGENDA-PROJECTION-READ-ONLY"
given:
  - "a household has scheduled plans, pending tasks, active routines and list items with due dates"
when: "the household timeline is requested for a date"
then:
  - "all four entry types appear for that date"
  - "no external calendar entries appear"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-HOUSEHOLD-TIMELINE"
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-ROUTINE"
  - "TERM-LIST-ITEM"
provenance:
  source: "openspec/specs/calendar/spec.md (Household Timeline Projection: Household timeline includes events, tasks, routines, and list items)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes the four sources of the household picture.

## Boundaries

None.
