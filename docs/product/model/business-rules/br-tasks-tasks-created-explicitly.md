---
id: BR-TASKS-TASKS-CREATED-EXPLICITLY
type: business-rule
title: Tasks are never generated from routines or plans
status: draft
applies-to:
  - BC-TASKS
  - UC-TASKS-CREATE-TASK
uses-terms:
  - TERM-TASK
  - TERM-ROUTINE
  - TERM-PLAN
  - TERM-TASK-ORIGIN
provenance:
  source: openspec/specs/tasks/spec.md (Purpose); docs/_legacy/04_contexts/tasks.md (Internal Entities, TaskOrigin; Calendar Context)
  confidence: high
  recovered-from: documentation
---

## Rule

Every task MUST originate from an explicit action (a person, or an external integration); the product MUST NOT create tasks automatically from routines or plans.

## Rationale

Keeps the task list a record of work someone decided to do, and keeps routines as definitions (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Internal Entities, TaskOrigin note).

## Examples

- A school trip plan does not create "prepare backpack"; a person creates that task (observed: docs/_legacy/04_contexts/tasks.md, Calendar Context).
- Calendar activity may later support task suggestions, but without creating tasks (observed: docs/_legacy/04_contexts/tasks.md, Domain Events Consumed, From Calendar).

## Exceptions

None.
