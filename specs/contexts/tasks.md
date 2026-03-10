# Context Spec — Tasks

## Purpose

Defines the functional scope of the Tasks context.

This context owns execution of household work.

---

## Responsibilities

- create tasks
- assign tasks
- complete tasks
- cancel tasks
- reschedule tasks
- define routines
- update and control routines

---

## Aggregates

- `Task`
- `Routine`

---

## Owned Concepts

- Task
- TaskAssignment
- TaskOrigin
- Routine

---

## Invariants

- every task belongs to one family
- a task has at most one primary assignee
- completed tasks cannot return to pending
- cancelled tasks cannot be completed
- routines must define valid recurrence rules

---

## Events Emitted

- `TaskCreated`
- `TaskAssigned`
- `TaskCompleted`
- `TaskCancelled`
- `TaskRescheduled`
- `RoutineCreated`
- `RoutineUpdated`
- `RoutinePaused`
- `RoutineResumed`

---

## Events Consumed

- `MemberAdded`
- `MemberRemoved`
- `EventScheduled`
- `EventRescheduled`
- `PrimaryOwnerAssigned`
- `ResponsibilityTransferred`

---

## Related Feature Specs

- create-task
- assign-task
- complete-task
- cancel-task
- reschedule-task
- create-routine
- update-routine
- pause-routine
- resume-routine