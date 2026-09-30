---
id: SB-CALENDAR-ADD-DUPLICATE-PARTICIPANT
type: structured-behaviour
title: "Adding the same participant twice is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
  - "BR-CALENDAR-PARTICIPANT-UNIQUE"
given:
  - "a person is already a participant in a plan"
when: "a household member adds the same person again"
then:
  - "the addition is rejected"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management: Duplicate participant is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that participants are unique within a plan.

## Boundaries

None.
