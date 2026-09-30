---
id: BR-TASKS-DISTINCT-EXECUTION-MODELS
type: business-rule
title: Tasks, list items and plans stay distinct
status: draft
applies-to:
  - BC-TASKS
uses-terms:
  - TERM-TASK
  - TERM-ROUTINE
  - TERM-LIST-ITEM
  - TERM-PLAN
provenance:
  source: openspec/specs/tasks/spec.md (Purpose); docs/_legacy/04_contexts/tasks.md (Tasks Is Not the Only Execution Model; Routine)
  confidence: high
  recovered-from: documentation
---

## Rule

A list item with a due date MUST NOT be treated as a task, and a recurring plan MUST NOT be treated as a routine.

## Rationale

Lightweight list execution, structured task execution and attendance commitments are different models; collapsing them loses what each is for (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Tasks Is Not the Only Execution Model, Routine).

## Examples

- "Football practice every Tuesday" is a plan; "pack sports bag before practice" is a task a person creates (observed: docs/_legacy/04_contexts/tasks.md, Routine).

## Exceptions

None.
