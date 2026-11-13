---
id: LC-PLAN
type: domain-lifecycle
title: Calendar Plan lifecycle
status: active
subject: TERM-PLAN
states:
  - id: SCHEDULED
    title: Scheduled
    initial: true
  - id: CANCELLED
    title: Cancelled
    terminal: true
transitions:
  - id: CANCEL
    title: Cancel a Plan
    from:
      - SCHEDULED
    to: CANCELLED
    trigger: A member cancels the Plan
    initiated-by:
      - ACT-HOUSEHOLD-MEMBER
    governed-by:
      - BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED
    realized-by:
      - UC-CALENDAR-CANCEL-PLAN
provenance:
  source: TERM-PLAN; UC-CALENDAR-CANCEL-PLAN; BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED; src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs
  confidence: high
  recovered-from: documentation
---

## Purpose

Show whether the household still intends a time commitment to take place.

## Invariants

A Plan belongs to one household. Cancellation keeps its history, closes it to further changes and does not create a completion or attendance record.

## State Semantics

Scheduled Plans are active commitments. Cancelled Plans remain in history and are no longer active.

## Transition Semantics

A member cancels a scheduled Plan. The domain records EventCancelled. Rescheduling changes timing while the Plan remains scheduled.

## Boundaries

Meal Plans have their own Draft, Active and Completed states. External calendar entries are read-only imports and never become Plans. Passing a Plan end time does not introduce a new state.
