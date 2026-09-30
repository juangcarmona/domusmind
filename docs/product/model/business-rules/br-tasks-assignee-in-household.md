---
id: BR-TASKS-ASSIGNEE-IN-HOUSEHOLD
type: business-rule
title: A task has at most one assignee from the same household
status: draft
applies-to:
  - UC-TASKS-CREATE-TASK
  - UC-TASKS-ASSIGN-TASK
uses-terms:
  - TERM-TASK
  - TERM-TASK-ASSIGNEE
  - TERM-MEMBER
provenance:
  source: "openspec/specs/tasks/spec.md (Task Creation, Task Assignment); docs/_legacy/04_contexts/tasks.md (Invariants, Assignment); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0011 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Rule

A task MUST have zero or one primary assignee at a time, and that assignee MUST be a person of the same household as the task.

## Rationale

Responsibility for a task must be unambiguous and must stay inside the household (observed: openspec/specs/tasks/spec.md, Task Assignment; docs/_legacy/04_contexts/tasks.md, Invariants, Assignment).

## Examples

- Creating or assigning a task with someone who is not in the household is rejected (observed: openspec/specs/tasks/spec.md, scenarios Creating a task with a non-member assignee is rejected; Assigning a task to a non-member is rejected).
- Assigning a task that already has an assignee replaces the previous one (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs, Reassign).

## Exceptions

None. Undecided, deferred (Q-0011): what happens to tasks assigned to a person who is removed from the household is left to the definition of member removal (V1.1).
