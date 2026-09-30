---
id: SB-TASKS-AGENDA-SHOWS-DUE-TASK
type: structured-behaviour
title: Task with a due date appears in the Agenda
status: draft
illustrates:
  - UC-TASKS-SEE-WORK-IN-AGENDA
given:
  - "A task exists with a due date"
when: A person views the Agenda for that date
then:
  - "The task appears in the day view for that date"
uses-terms:
  - TERM-TASK
  - TERM-AGENDA
provenance:
  source: openspec/specs/tasks/spec.md (Agenda Projection, scenario Task with due date appears in Agenda)
  confidence: high
  recovered-from: documentation
---

## Intent

Dated tasks surface on their day.

## Boundaries

Does not define how overdue or completed tasks are displayed; that belongs to the Agenda.
