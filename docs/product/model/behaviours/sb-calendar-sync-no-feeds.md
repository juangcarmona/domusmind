---
id: SB-CALENDAR-SYNC-NO-FEEDS
type: structured-behaviour
title: "A sync with no selected calendars does nothing"
status: draft
illustrates:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT"
given:
  - "a connection has no selected calendars"
when: "a sync is triggered"
then:
  - "no entries are imported"
  - "the sync completes without error"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization: Sync with no selected feeds is a no-op)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that an empty selection is valid.

## Boundaries

None.
