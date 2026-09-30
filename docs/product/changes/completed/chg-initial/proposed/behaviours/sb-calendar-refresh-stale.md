---
id: SB-CALENDAR-REFRESH-STALE
type: structured-behaviour
title: "A stale connection is refreshed automatically"
status: draft
illustrates:
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
given:
  - "a connection's last successful sync is older than the threshold"
when: "the Calendar Sync Scheduler evaluates connections"
then:
  - "that connection is synchronized"
  - "its sync time is updated"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh: Stale connection is refreshed automatically); interview: product owner decision Q-0047 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that no one needs to sync manually for the Agenda to stay current.

## Boundaries

Undecided (Q-0047): the allowed interval values and the staleness threshold that triggers catch-up are decided later.
