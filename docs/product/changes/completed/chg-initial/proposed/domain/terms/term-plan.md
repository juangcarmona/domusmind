---
id: TERM-PLAN
type: domain-term
title: "Plan"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Event"
  - "Calendar Event"
uses-terms:
  - "TERM-HOUSEHOLD"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-REMINDER"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/calendar/spec.md (Purpose, Event Scheduling); docs/_legacy/04_contexts/calendar.md (Event, Schedule Semantics, Ubiquitous Language Notes); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTime.cs; interview: product owner decision Q-0044 (E-0158); interview: product owner decision Q-0045 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A time-bound commitment that affects one or more people in the household: something scheduled in time, such as a school activity, a medical appointment, a trip or a family gathering (observed: legacy calendar.md, Event examples).

A plan belongs to exactly one household, has a title and a start, an optional end after the start, optional participants, optional reminders and an optional area reference. It starts as scheduled and can be cancelled; a cancelled plan stays in history but is no longer active (observed: calendar spec, Event Scheduling and Event Cancellation).

A plan can also be all-day (a date without a time of day) or span several days; both are part of the product (decided: Q-0045; observed: EventTime.cs). A plan carries an optional description and a colour (observed: CalendarEvent.cs).

Recurrence rules (e.g. "football practice every Tuesday") are described in the legacy schedule model but have no current behaviour; recurring-plan management is planned for V2 (decided: Q-0044) (observed: legacy calendar.md, Schedule Semantics; calendar spec, Note 1; inferred: absent from the domain code).

## Distinguish From

- **Task**: operational work that someone does; a plan is a time commitment. Preparing a backpack for a school trip is a task, the trip is a plan (observed: legacy calendar.md, Tasks Context).
- **Routine**: a recurring piece of operational work owned by Tasks; recurring fixed-time activities remain plans (observed: legacy calendar.md, Tasks Context).
- **External calendar entry**: an imported, read-only occurrence from a provider calendar; it is never a plan (observed: calendar spec, Purpose).

## Usage

"Plan" is the household-facing name; "Event" is the name in the Calendar domain documents and code, kept here as a synonym (observed: calendar spec, Purpose; legacy ubiquitous-language.md, Plan). Plans appear in the Agenda in both Household and Member scope.
