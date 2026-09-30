---
id: BR-TASKS-ROUTINES-PROJECTED
type: business-rule
title: Routines are projected, not turned into tasks
status: draft
applies-to:
  - BC-TASKS
  - UC-TASKS-CREATE-ROUTINE
  - UC-TASKS-SEE-WORK-IN-AGENDA
uses-terms:
  - TERM-ROUTINE
  - TERM-ROUTINE-OCCURRENCE
  - TERM-AGENDA
  - TERM-TASK
provenance:
  source: openspec/specs/tasks/spec.md (Purpose, Notes 1); docs/_legacy/04_contexts/tasks.md (Routine Projection Model)
  confidence: high
  recovered-from: documentation
---

## Rule

A routine MUST NOT produce tasks or stored occurrences; it MUST be shown in read surfaces on the fly, on each date matching its recurrence, while it is active.

## Rationale

Views always reflect the routine's current definition without background scheduling or persisted occurrences (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model; openspec/specs/tasks/spec.md, Purpose).

## Examples

- "Weekly Trash, every Tuesday, household scope" appears on each Tuesday in the Agenda and no task is created (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model).

## Exceptions

None. Feature specs that mention "generated tasks" are stale (observed: openspec/specs/tasks/spec.md, Notes 1).
