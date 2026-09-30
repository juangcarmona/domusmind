---
id: SB-TASKS-UPDATE-ROUTINE
type: structured-behaviour
title: Routine definition updated
status: draft
illustrates:
  - UC-TASKS-UPDATE-ROUTINE
given:
  - "An active routine exists"
when: A person provides updated fields
then:
  - "The routine definition is updated"
  - "Future Agenda projections reflect the new definition"
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
provenance:
  source: openspec/specs/tasks/spec.md (Routine Update, scenario Routine definition is updated)
  confidence: high
  recovered-from: documentation
---

## Intent

Changes apply going forward only.

## Boundaries

Does not assert anything about updating a paused routine.
