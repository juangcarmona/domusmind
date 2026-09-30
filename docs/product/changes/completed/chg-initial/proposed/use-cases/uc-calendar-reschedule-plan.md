---
id: UC-CALENDAR-RESCHEDULE-PLAN
type: use-case
title: "Reschedule a plan"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-PLAN-END-AFTER-START"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Rescheduling); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Reschedule)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Move a plan to a new time without losing who is involved or its reminders (observed: calendar spec).

## Trigger

A plan's time changes, for example an appointment is moved.

## Preconditions

- The plan exists and is scheduled, not cancelled (observed: calendar spec).

## Main Flow

1. The person gives the plan a new start and optionally a new end.
2. DomusMind checks the new end is after the new start.
3. DomusMind updates the plan's schedule; its identity, participants and reminders stay as they were (observed: calendar spec).

## Alternative Flows

None recorded in the sources.

## Failure Conditions

- The plan is cancelled: rescheduling is rejected (observed: calendar spec).
- The new end is before the new start: rejected (observed: calendar spec).

## Postconditions

- The plan appears at its new time in the Agenda (inferred from agenda.md, Data).
