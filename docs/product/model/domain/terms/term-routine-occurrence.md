---
id: TERM-ROUTINE-OCCURRENCE
type: domain-term
title: Routine Occurrence
status: draft
defined-in: BC-TASKS
synonyms:
  - "Projected occurrence"
  - Projection
uses-terms:
  - TERM-ROUTINE
  - TERM-AGENDA
  - TERM-TASK
provenance:
  source: openspec/specs/tasks/spec.md (Agenda Projection, Routine Resume); docs/_legacy/04_contexts/tasks.md (Routine Projection Model)
  confidence: high
  recovered-from: documentation
---

## Definition

An entry that represents an active routine on one date matching its recurrence. It is computed when a view is built and is never stored: no background job, no persisted occurrence records, so it always reflects the routine's current definition (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model; openspec/specs/tasks/spec.md, Routine Creation, Agenda Projection).

## Distinguish From

- Task (TERM-TASK): an occurrence is not a task and is not turned into one (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model).

## Usage

Appears in the Agenda as a projected occurrence with a cycle cue (observed: openspec/specs/tasks/spec.md, Agenda Projection; context: docs/_legacy/00_product/surfaces/agenda.md). Occurrences missed while a routine was paused are not recreated (observed: openspec/specs/tasks/spec.md, Routine Resume).
