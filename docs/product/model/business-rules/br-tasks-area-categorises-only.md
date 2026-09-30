---
id: BR-TASKS-AREA-CATEGORISES-ONLY
type: business-rule
title: Areas categorise tasks but do not change them
status: draft
applies-to:
  - BC-TASKS
  - UC-TASKS-CREATE-TASK
  - UC-TASKS-CREATE-ROUTINE
uses-terms:
  - TERM-AREA
  - TERM-TASK
provenance:
  source: "docs/_legacy/04_contexts/tasks.md (Invariants, Ownership Boundary; Boundaries With Other Contexts); openspec/specs/tasks/spec.md (Task Creation, Notes 7); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Routine.cs; interview: product owner decision Q-0010 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Rule

A task or routine MAY reference one area for grouping; only the Tasks part of the product changes a task's state, and area ownership never modifies a task directly.

## Rationale

Tasks owns execution; Areas owns ownership rules (observed: docs/_legacy/04_contexts/tasks.md, Invariants, Ownership Boundary; Boundaries With Other Contexts).

## Examples

- A task "pay electricity bill" grouped under the Maintenance area (observed: docs/_legacy/04_contexts/tasks.md, Responsibility Context examples: food, school, maintenance).
- Routines can also reference an area in the current model (observed: src/backend/DomusMind.Domain/Tasks/Routine.cs).

## Exceptions

When an Area is archived, tasks and routines lose their Area reference (decided: Q-0010; BR-TASKS-ARCHIVED-AREA-CLEARED). What happens when an Area is deleted is not specified (observed: openspec/specs/tasks/spec.md, Notes 7).
