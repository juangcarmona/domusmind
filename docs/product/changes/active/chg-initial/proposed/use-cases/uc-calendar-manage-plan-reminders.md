---
id: UC-CALENDAR-MANAGE-PLAN-REMINDERS
type: use-case
title: "Add or remove plan reminders"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-REMINDER-OFFSET-UNIQUE"
  - "BR-CALENDAR-REMINDER-BEFORE-START"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
uses-terms:
  - "TERM-PLAN"
  - "TERM-REMINDER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders); docs/_legacy/04_contexts/calendar.md (Reminder); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddReminder, RemoveReminder); interview: product owner decision Q-0043 (E-0158); interview: product owner decision Q-0046 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Decide how long before a plan the household should be reminded (observed: calendar spec, Event Reminders).

## Trigger

A person wants a prompt before a plan, for example 24 hours before a trip.

## Preconditions

- The plan exists (observed: calendar spec) and is not cancelled (decided: Q-0043).

## Main Flow

1. The person adds a reminder as an amount of time before the plan's start.
2. DomusMind checks the plan has no reminder with that offset.
3. DomusMind adds the reminder to the plan (observed: calendar spec).

## Alternative Flows

- 1a. The person removes an existing reminder; the plan and its other reminders are unchanged (observed: calendar spec).

## Failure Conditions

- A reminder with the same offset already exists: rejected (observed: calendar spec).
- Removing an offset the plan does not have: rejected (observed: calendar spec).
- A zero or negative offset: rejected (observed: CalendarEvent.cs).
- The plan is cancelled: adding or removing a reminder is rejected (decided: Q-0043).

## Postconditions

- The plan's reminders are defined. Delivering the reminder is not part of this use case (observed: calendar spec, delivery belongs to infrastructure).

Undecided, deferred (Q-0046): whether DomusMind delivers reminder notifications to people and through which channel is left to a later decision; until then a reminder is informational.
