---
id: BR-CALENDAR-REMINDER-BEFORE-START
type: business-rule
title: "A reminder is an offset before the plan's start"
status: draft
applies-to:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
uses-terms:
  - "TERM-REMINDER"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders); docs/_legacy/04_contexts/calendar.md (Reminder Integrity); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddReminder)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A plan reminder is defined relative to the plan's start, as a positive amount of time before it; a zero or negative offset is rejected (observed: calendar spec, "offsets relative to the event's start time"; CalendarEvent.cs rejects offsets not greater than zero).

## Rationale

Reminders follow the plan when it is rescheduled because they reference the schedule, not a fixed moment (inferred from legacy calendar.md, "reminders must reference the event schedule", and calendar spec, reminders unchanged on reschedule).

## Examples

- 24 hours before, 2 hours before and 30 minutes before are valid reminders (observed: legacy calendar.md).

## Exceptions

None.
