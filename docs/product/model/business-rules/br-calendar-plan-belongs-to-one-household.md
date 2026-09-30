---
id: BR-CALENDAR-PLAN-BELONGS-TO-ONE-HOUSEHOLD
type: business-rule
title: "Every plan belongs to exactly one household"
status: draft
applies-to:
  - "BC-CALENDAR"
  - "UC-CALENDAR-SCHEDULE-PLAN"
uses-terms:
  - "TERM-PLAN"
  - "TERM-HOUSEHOLD"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling); docs/_legacy/04_contexts/calendar.md (Identity invariants); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A plan belongs to exactly one household for its whole life and keeps a stable identity when rescheduled (observed: calendar spec, Event Scheduling and Event Rescheduling; legacy calendar.md, Identity).

## Rationale

Plans are household coordination state; they are never shared across households (inferred).

## Examples

- A plan scheduled in the Garcia household is only visible in that household's Agenda (inferred).

## Exceptions

None.
