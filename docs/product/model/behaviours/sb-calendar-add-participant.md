---
id: SB-CALENDAR-ADD-PARTICIPANT
type: structured-behaviour
title: "A household member is added to a plan"
status: draft
illustrates:
  - "UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS"
given:
  - "a plan exists in scheduled status"
  - "a household member (of any role) is not yet a participant"
when: "a household member adds that person as a participant"
then:
  - "the person is in the plan's participants"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-MEMBER-ROLE"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management: Participant is added to an event); interview: product owner decision Q-0042 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes who can take part in a plan.

## Boundaries

Participants are always household members, of any role; pets take part as members with the Pet role (decided: Q-0042).
