---
id: TERM-TASK
type: domain-term
title: Task
status: draft
defined-in: BC-TASKS
synonyms:
  - HouseholdTask
uses-terms:
  - TERM-TASK-STATUS
  - TERM-TASK-ASSIGNEE
  - TERM-MEMBER
provenance:
  source: "openspec/specs/tasks/spec.md (Purpose, Task Creation); docs/_legacy/04_contexts/tasks.md (Aggregate Roots, Task; Invariants); src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs; interview: product owner decision Q-0005 (E-0158); interview: product owner decision Q-0058 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

A concrete, actionable unit of household work with an optional assignee, an optional due date and a defined lifecycle (observed: openspec/specs/tasks/spec.md, Purpose, Task Creation). Examples: buy groceries, prepare school bag, pay electricity bill, bring documents (observed: docs/_legacy/04_contexts/tasks.md, Aggregate Roots, Task).

A task belongs to exactly one household (observed: docs/_legacy/04_contexts/tasks.md, Invariants, Identity). Only its title is required when it is created; it starts pending (observed: openspec/specs/tasks/spec.md, Task Creation). A due date may be a date only or a date and time to the minute, and a task may carry a description and a colour (decided: Q-0058; observed: src/backend/DomusMind.Domain/Tasks/ValueObjects/TaskSchedule.cs, HouseholdTask.cs). Its lifecycle is pending, then completed or cancelled (decided: Q-0005).

## Distinguish From

- List item (TERM-LIST-ITEM): a list item carrying a due date is not a Task; a Task carries structured execution semantics a list item does not require (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Tasks Is Not the Only Execution Model).
- Routine (TERM-ROUTINE): a routine is a recurring definition; it never produces tasks. A manually created task may look like a routine entry in the Agenda, but it comes from a direct user action (observed: docs/_legacy/04_contexts/tasks.md, Routine Projection Model).
- Plan (TERM-PLAN): a fixed-time attendance commitment owned by Calendar; "pack sports bag before practice" is a task, "football practice every Tuesday" is a plan (observed: docs/_legacy/04_contexts/tasks.md, Routine).

## Usage

"Task" is both the household-facing and the internal term (observed: docs/_legacy/04_contexts/tasks.md, Aggregate Roots, Task). The internal model names it HouseholdTask (observed: src/backend/DomusMind.Domain/Tasks/HouseholdTask.cs). Tasks with a due date appear in the Agenda on that date (observed: openspec/specs/tasks/spec.md, Agenda Projection).
