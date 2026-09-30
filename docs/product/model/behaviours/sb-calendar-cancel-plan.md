---
id: SB-CALENDAR-CANCEL-PLAN
type: structured-behaviour
title: "A cancelled plan leaves active planning"
status: draft
illustrates:
  - "UC-CALENDAR-CANCEL-PLAN"
given:
  - "a plan exists in scheduled status"
when: "a household member cancels the plan"
then:
  - "the plan's status becomes cancelled"
  - "the plan is no longer considered active"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Cancellation: Event is cancelled)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that cancellation keeps the plan in history but out of active planning.

## Boundaries

Does not assert how, or whether, cancelled plans appear in the Agenda.
