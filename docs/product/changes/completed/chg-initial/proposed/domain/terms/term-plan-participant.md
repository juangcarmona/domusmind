---
id: TERM-PLAN-PARTICIPANT
type: domain-term
title: "Plan Participant"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Participant"
  - "Event Participant"
uses-terms:
  - "TERM-PLAN"
  - "TERM-MEMBER"
  - "TERM-MEMBER-ROLE"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management); docs/_legacy/04_contexts/calendar.md (Participant); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs; interview: product owner decision Q-0042 (E-0158); interview: product owner decision Q-0041 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Definition

A household member, of any role, who attends or is affected by a plan; pets take part as members with the Pet role and there are no separate participant kinds (decided: Q-0042). The domain code references participants by household member identity only (observed: CalendarEvent.cs).

Each participant appears at most once in a plan and must belong to the plan's household (observed: calendar spec).

## Distinguish From

- **Area owner**: accountability for an area of the household (Responsibilities), not presence at a plan (observed: legacy calendar.md, Ownership Boundary).
- **Task assignee**: the person expected to do a task; a participant is who needs to be where, and when (observed: legacy calendar.md, Participant).

## Usage

Participants answer the coordination question "who needs to be where, and when?" (observed: legacy calendar.md, Participant). Together with household plans without participants, they decide which plans appear in a person's Member-scope Agenda (decided: Q-0041) and are shown in the Agenda inspector for a plan (observed: calendar spec, Member Agenda Projection; agenda.md, Inspector).
