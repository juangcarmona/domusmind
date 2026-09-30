---
id: FR-CALENDAR-SCHEDULE-PLAN
type: functional-requirement
title: "Schedule a plan"
status: draft
derived-from:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "BR-CALENDAR-PLAN-REQUIRES-TITLE-AND-START"
  - "BR-CALENDAR-PLAN-END-AFTER-START"
  - "BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD"
verification:
  - scenario-ref: "SB-CALENDAR-SCHEDULE-PLAN-MINIMUM"
  - scenario-ref: "SB-CALENDAR-SCHEDULE-PLAN-END-BEFORE-START"
  - scenario-ref: "SB-CALENDAR-SCHEDULE-PLAN-UNKNOWN-PARTICIPANT"
  - scenario-ref: "SB-CALENDAR-SCHEDULE-ALL-DAY-PLAN"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-AREA"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; src/backend/DomusMind.Domain/Calendar/ValueObjects/EventTime.cs; interview: product owner decision Q-0045 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member schedule a plan with a title and a start as the only required inputs, where the start is a date and time or, for an all-day plan, a date alone; it MUST also accept multi-day plans spanning a start and an end date (decided: Q-0045; observed: EventTime.cs). It MUST accept an optional end (after the start), optional participants from the household and an optional area reference. A new plan MUST start in scheduled status and belong to exactly one household (observed: calendar spec).

The openspec requires a start time; the decision overrides it for all-day and multi-day plans (observed: calendar spec, Event Scheduling; decided: Q-0045).

## Rationale

Capturing a plan must be quicker than remembering it, and the household's plans are the shared source of truth for time (observed: calendar spec, Purpose).
