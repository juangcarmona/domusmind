---
id: BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD
type: business-rule
title: "Participants must belong to the plan's household"
status: draft
applies-to:
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
uses-terms:
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-HOUSEHOLD"
  - "TERM-MEMBER"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Scheduling, invalid participant scenario); docs/_legacy/04_contexts/calendar.md (Participation, Ownership Boundary)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

A participant must be a member of the plan's household; unknown participants are not allowed (observed: calendar spec; legacy calendar.md, Participation).

## Rationale

Participants answer who in this household needs to be where (observed: legacy calendar.md, Participant).

## Examples

- Adding someone from another household to a plan is rejected with a validation error (observed: calendar spec).

## Exceptions

None.
