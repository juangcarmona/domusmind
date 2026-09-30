---
id: SB-CALENDAR-MANUAL-SYNC
type: structured-behaviour
title: "A manual sync imports entries from selected calendars"
status: draft
illustrates:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
given:
  - "an active connection has at least one selected calendar"
when: "the member triggers a sync"
then:
  - "entries from all selected calendars within the horizon are imported or updated"
  - "the sync state of each calendar is updated"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization: Manual sync imports entries from selected feeds)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that a manual sync brings the person's entries up to date.

## Boundaries

None.
