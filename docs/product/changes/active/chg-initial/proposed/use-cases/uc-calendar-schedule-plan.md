---
id: UC-CALENDAR-SCHEDULE-PLAN
type: use-case
title: "Schedule a plan"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START"
  - "BR-CALENDAR-PLAN-END-AFTER-START"
  - "BR-CALENDAR-PLAN-BELONGS-TO-ONE-HOUSEHOLD"
  - "BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-HOUSEHOLD"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling); docs/_legacy/04_contexts/calendar.md (Commands: ScheduleEvent); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTime.cs; interview: product owner decision Q-0045 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Put a time-bound household commitment on the household's calendar so everyone can see what is happening, when, and who is involved (observed: calendar spec, Purpose).

## Trigger

A person wants to record something scheduled, for example a medical appointment or a school activity, from the Agenda or elsewhere (observed: legacy calendar.md, Event examples; agenda.md, Creating).

## Preconditions

- The household exists and the person belongs to it (observed: calendar spec, "a family exists").

## Main Flow

1. The person gives the plan a title and a start: a date and time, or a date alone for an all-day plan (decided: Q-0045).
2. Optionally the person adds an end, participants from the household and an area for grouping (observed: calendar spec, optional inputs).
3. DomusMind checks the end is after the start and every participant belongs to the household.
4. DomusMind records the plan as scheduled in that household (observed: calendar spec).

## Alternative Flows

- 1a. The person schedules an all-day or multi-day plan with dates but no time of day (observed: EventTime.cs; decided: Q-0045).
- 2a. The person also adds a description or picks a colour (observed: CalendarEvent.cs).

## Failure Conditions

- The end is before the start: the plan is not created and a validation error is shown (observed: calendar spec).
- A participant does not belong to the household: the plan is not created and a validation error is shown (observed: calendar spec).
- The title is empty or longer than 200 characters: rejected (observed: EventTitle.cs).

## Postconditions

- A scheduled plan exists in the household and appears in the Agenda for its date (observed: calendar spec; agenda.md, Data).
