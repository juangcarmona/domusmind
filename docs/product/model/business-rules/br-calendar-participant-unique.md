---
id: BR-CALENDAR-PARTICIPANT-UNIQUE
type: business-rule
title: "A person takes part in a plan at most once"
status: draft
applies-to:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
uses-terms:
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management); docs/_legacy/04_contexts/calendar.md (Participation); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddParticipant, RemoveParticipant)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Each participant is unique within a plan: adding someone already taking part is rejected, and removing someone who is not taking part is rejected (observed: calendar spec; CalendarEvent.cs).

## Rationale

Keeps the participant list a faithful answer to who is involved (inferred).

## Examples

- Adding Ana twice to "Swimming lesson" is rejected (observed: calendar spec).
- Removing Leo from a plan he is not in is rejected (observed: calendar spec).

## Exceptions

None.
