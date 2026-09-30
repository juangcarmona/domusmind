---
id: BR-TASKS-EXPLICIT-ASSIGNMENT
type: business-rule
title: Tasks are only assigned explicitly
status: draft
applies-to:
  - UC-TASKS-ASSIGN-TASK
  - BC-TASKS
uses-terms:
  - TERM-TASK-ASSIGNEE
  - TERM-AREA
provenance:
  source: "openspec/specs/tasks/spec.md (Task Assignment); conflicts with docs/_legacy/04_contexts/tasks.md (Domain Events Consumed); interview: product owner decision Q-0008 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

A task's assignment MUST always be an explicit action by a person; the product MUST NOT assign tasks automatically, even when area ownership or a plan motivates the assignment.

## Rationale

Keeps who does what a deliberate household decision (observed: openspec/specs/tasks/spec.md, Task Assignment).

## Examples

- The owner of the Food area is a likely assignee for "buy groceries", but someone still has to assign it (observed: openspec/specs/tasks/spec.md, Task Assignment).

## Exceptions

None. Assignment is never automatic (decided: Q-0008). The legacy context's "suggest or auto-assign tasks" reaction to area ownership changes is dropped as auto-assignment (observed: docs/_legacy/04_contexts/tasks.md, Domain Events Consumed, From Responsibility); suggesting an assignee without assigning may be a future capability (decided: Q-0008).
