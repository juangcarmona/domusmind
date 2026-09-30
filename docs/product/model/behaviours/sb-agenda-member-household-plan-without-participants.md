---
id: SB-AGENDA-MEMBER-HOUSEHOLD-PLAN-WITHOUT-PARTICIPANTS
type: structured-behaviour
title: "A household plan without participants appears in each person's Agenda"
status: draft
illustrates:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
  - "BR-AGENDA-MEMBER-SCOPE-PLANS"
given:
  - "the household has a plan with no participants on a date"
  - "the household has a plan on the same date whose only participant is another person"
when: "a person's Member-scope Agenda is requested for that date"
then:
  - "the plan without participants appears"
  - "the plan of the other person does not appear"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
provenance:
  source: "openspec/specs/calendar/spec.md (Member Agenda Projection); interview: product owner decision Q-0041 (E-0158)"
  confidence: "high"
  recovered-from: "interview"
---

## Intent

Establishes which plans a person's own Agenda shows (decided: Q-0041).

## Boundaries

Does not assert how plans are ordered or presented within the day.
