---
id: SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN
type: structured-behaviour
title: "Opening one's Agenda catches up a stale connection"
status: draft
illustrates:
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
given:
  - "a member opens their Agenda in Member scope"
  - "one of their connections is stale"
when: "the Agenda loads"
then:
  - "a catch-up sync is triggered for the stale connection"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh: Catch-up sync fires when Agenda opens with stale state)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes the Agenda-open catch-up trigger.

## Boundaries

Does not assert that the Agenda waits for the catch-up to finish before showing entries.
