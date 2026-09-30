---
id: SB-CALENDAR-REMOVE-NON-PARTICIPANT
type: structured-behaviour
title: "Removing someone who is not a participant is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
  - "BR-CALENDAR-PARTICIPANT-UNIQUE"
given:
  - "a person is not a participant in a plan"
when: "a household member tries to remove that person"
then:
  - "the removal is rejected"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management: Removing a non-participant is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that removal only applies to actual participants.

## Boundaries

None.
