---
id: BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START
type: business-rule
title: "A plan needs a title and a start"
status: draft
applies-to:
  - "UC-CALENDAR-SCHEDULE-PLAN"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTitle.cs; interview: product owner decision Q-0045 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A plan can be scheduled with only a title and a start, where the start is a date and time or, for an all-day or multi-day plan, a date without a time of day (decided: Q-0045); everything else (end, participants, reminders, area) is optional (observed: calendar spec, Event Scheduling). The domain code rejects an empty title and a title longer than 200 characters (observed: EventTitle.cs).

## Rationale

Keeps capturing a plan cheaper than remembering it (inferred from the product's low-friction capture goal).

## Examples

- "Dentist" at 09:00 on Tuesday is a valid plan with no end and no participants (observed: calendar spec, minimum inputs scenario).
- A plan with a blank title is rejected (observed: EventTitle.cs).
- "School trip" on Friday with no time of day is a valid all-day plan (decided: Q-0045).

## Exceptions

None. All-day and multi-day plans are part of the product and need a start date but no start time (decided: Q-0045; observed: EventTime.cs); this overrides the openspec's start-time minimum (observed: calendar spec, Event Scheduling).
