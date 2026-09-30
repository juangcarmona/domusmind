---
id: SB-CALENDAR-REMOVE-PARTICIPANT
type: structured-behaviour
title: "A participant is removed from a plan"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
given:
  - "a person is a participant in a scheduled plan"
when: "a household member removes that participant"
then:
  - "the person is no longer in the plan's participants"
  - "the plan and its other participants remain unchanged"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management: Participant is removed from an event)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that removal affects only that participant.

## Boundaries

None.
