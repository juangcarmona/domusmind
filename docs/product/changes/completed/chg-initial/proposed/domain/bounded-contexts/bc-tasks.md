---
id: BC-TASKS
type: bounded-context
title: Tasks
status: draft
provenance:
  source: "openspec/specs/tasks/spec.md (Purpose, Requirements); docs/_legacy/04_contexts/tasks.md (Purpose, Boundaries With Other Contexts, Design Notes); src/backend/DomusMind.Domain/Tasks/**; interview: product owner decision Q-0006 (E-0158); interview: product owner decision Q-0008 (E-0158); interview: product owner decision Q-0010 (E-0158); interview: product owner decision Q-0011 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Responsibility

Tasks is the household's structured execution layer. It answers what explicit work needs to be done, who is responsible for doing it, when it should happen and whether it has been completed (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Purpose).

It covers:
- tasks: concrete, assignable, lifecycle-driven units of household work, created explicitly (observed: openspec/specs/tasks/spec.md, Purpose);
- routines: recurring definitions of operational household work, projected into read surfaces on matching dates rather than turned into tasks (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Routine Projection Model);
- task assignment to a person of the household, task completion, cancellation and rescheduling (observed: openspec/specs/tasks/spec.md, Requirements);
- categorising tasks and routines by household area (observed: docs/_legacy/04_contexts/tasks.md, Boundaries With Other Contexts, Responsibility Context; src/backend/DomusMind.Domain/Tasks, area reference on tasks and routines).

Tasks represents execution, not planning or attendance scheduling (observed: docs/_legacy/04_contexts/tasks.md, Design Notes).

## Language

Operational, execution-oriented language: Task, Routine, Assignee, Task Status, Routine Recurrence, Routine Scope and projected Routine Occurrence. Routine kind is not part of the product (decided: Q-0006). "Task" is both the household-facing and the internal term; no translation is needed (observed: docs/_legacy/04_contexts/tasks.md, Aggregate Roots, Task).

Terms to avoid unless explicitly modelled: todo, reminder, checklist item, activity (observed: docs/_legacy/04_contexts/tasks.md, Ubiquitous Language Notes).

## Boundaries

- A list item that carries a due date is not a Task; a Task is not a list item with more fields. Lightweight execution belongs to Lists (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Tasks Is Not the Only Execution Model).
- A recurring Plan is not a Routine. Fixed-time attendance commitments (school, football practice, a doctor appointment) belong to Calendar; the work around them belongs here only when someone creates it explicitly (observed: openspec/specs/tasks/spec.md, Purpose; docs/_legacy/04_contexts/tasks.md, Routine and Calendar Context).
- Tasks does not own who belongs to the household, nor who owns an area; it only references them (observed: docs/_legacy/04_contexts/tasks.md, Boundaries With Other Contexts).
- The Agenda is not part of Tasks. Tasks and Routines appear there, but the Agenda never creates or modifies them (observed: openspec/specs/tasks/spec.md, Agenda Projection).

## External Relationships

- Household (BC-FAMILY): a task's assignee and a routine's target people must be people of the same household (observed: openspec/specs/tasks/spec.md, Task Assignment, Routine Creation). The legacy context says Tasks reacts when a person is added or removed, possibly to validate or remove invalid assignments (observed: docs/_legacy/04_contexts/tasks.md, Domain Events Consumed); Undecided, deferred (Q-0011): what happens to tasks assigned to a person who is removed from the household is left to the definition of member removal (V1.1).
- Areas (BC-RESPONSIBILITIES): areas may categorise tasks for grouping; they cannot modify them. Area ownership rules stay in Areas (observed: docs/_legacy/04_contexts/tasks.md, Boundaries With Other Contexts). Area ownership never assigns tasks automatically; assignment is always explicit, and assignee suggestions may be a future capability (decided: Q-0008). The legacy context's auto-assignment idea is dropped (observed: docs/_legacy/04_contexts/tasks.md, Domain Events Consumed). When an Area is archived, tasks and routines lose their Area reference (decided: Q-0010).
- Calendar (BC-CALENDAR): owns Plans; Tasks owns the work associated with a Plan when that work is explicitly created (observed: docs/_legacy/04_contexts/tasks.md, Calendar Context). Tasks with a due date and active routines are projected into the Agenda, the unified read surface described in TERM-AGENDA (observed: openspec/specs/tasks/spec.md, Agenda Projection).
- Lists (BC-LISTS): a separate, lightweight execution model; the two must not be collapsed (observed: docs/_legacy/04_contexts/tasks.md, Tasks Is Not the Only Execution Model).
