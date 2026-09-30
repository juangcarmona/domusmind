---
id: BR-CALENDAR-REMINDER-OFFSET-UNIQUE
type: business-rule
title: "Reminder offsets are unique per plan"
status: draft
applies-to:
  - "UC-CALENDAR-MANAGE-PLAN-REMINDERS"
uses-terms:
  - "TERM-REMINDER"
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Reminders); docs/_legacy/04_contexts/calendar.md (Reminder Integrity); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddReminder, RemoveReminder)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A plan holds at most one reminder per offset; adding a second reminder with the same offset is rejected, and removing an offset that does not exist is rejected (observed: calendar spec; CalendarEvent.cs).

## Rationale

Two identical reminders would notify twice for no reason (inferred).

## Examples

- A plan with a "30 minutes before" reminder rejects another "30 minutes before" (observed: calendar spec).

## Exceptions

None.
