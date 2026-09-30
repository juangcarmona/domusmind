---
id: SB-CALENDAR-REFRESH-SKIP-FRESH
type: structured-behaviour
title: "A recently synced connection is skipped"
status: draft
illustrates:
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
given:
  - "a connection was synchronized recently"
when: "the Calendar Sync Scheduler evaluates connections"
then:
  - "that connection is skipped"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh: Already-fresh connection is skipped)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that background refresh does not resync fresh data.

## Boundaries

None.
