---
id: FR-CALENDAR-MANAGE-PLAN-PARTICIPANTS
type: functional-requirement
title: "Add and remove plan participants"
status: draft
derived-from:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
  - "BR-CALENDAR-PARTICIPANT-UNIQUE"
  - "BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
verification:
  - scenario-ref: "SB-CALENDAR-ADD-PARTICIPANT"
  - scenario-ref: "SB-CALENDAR-ADD-DUPLICATE-PARTICIPANT"
  - scenario-ref: "SB-CALENDAR-ADD-PARTICIPANT-CANCELLED-PLAN"
  - scenario-ref: "SB-CALENDAR-REMOVE-PARTICIPANT"
  - scenario-ref: "SB-CALENDAR-REMOVE-NON-PARTICIPANT"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-MEMBER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member add household members as participants of a scheduled plan and remove them, keeping each participant unique within the plan, and MUST reject adding or removing participants on a cancelled plan (observed: calendar spec; decided: Q-0043).

## Rationale

Participants answer who needs to be where, and when, and drive each person's Agenda (observed: legacy calendar.md; calendar spec, Member Agenda Projection).
