---
id: BR-CALENDAR-PLAN-END-AFTER-START
type: business-rule
title: "A plan's end must come after its start"
status: draft
applies-to:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "UC-CALENDAR-RESCHEDULE-PLAN"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling, Event Rescheduling); docs/_legacy/04_contexts/calendar.md (Schedule invariants); src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTime.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

When a plan has an end, the end must be after the start, both when scheduling and when rescheduling (observed: calendar spec). The domain code rejects a timed range whose end is equal to or before its start, and a multi-day plan whose end date is before its start date (observed: EventTime.cs).

## Rationale

A plan with a negative duration cannot be placed on the Agenda (inferred).

## Examples

- Start 10:00, end 09:00: rejected with a validation error (observed: calendar spec).
- Rescheduling to start 14:00, end 13:30: rejected (observed: calendar spec).

## Exceptions

None.
