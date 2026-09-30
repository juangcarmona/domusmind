---
id: SB-CALENDAR-ADD-PARTICIPANT-CANCELLED-PLAN
type: structured-behaviour
title: "No participants can be added to a cancelled plan"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
given:
  - "a plan is cancelled"
when: "a household member tries to add a participant"
then:
  - "the addition is rejected"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management: Participant cannot be added to a cancelled event); interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that cancelled plans do not gain participants.

## Boundaries

Removing participants from a cancelled plan is also rejected under the same rule (decided: Q-0043); this example covers only the addition.
