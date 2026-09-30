---
id: SB-CALENDAR-SYNC-INVALID-STATE
type: structured-behaviour
title: "Invalid sync state falls back to a fresh load"
status: draft
illustrates:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
given:
  - "a calendar's stored incremental sync state is invalid or stale"
when: "a sync is triggered"
then:
  - "the calendar's local entries are cleared"
  - "a fresh bounded load is performed"
  - "new sync state is stored"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization: Sync with invalid delta cursor falls back to fresh load)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that sync recovers on its own from bad state.

## Boundaries

None.
