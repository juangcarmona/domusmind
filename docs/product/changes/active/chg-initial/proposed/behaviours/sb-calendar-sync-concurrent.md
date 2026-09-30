---
id: SB-CALENDAR-SYNC-CONCURRENT
type: structured-behaviour
title: "A connection is not synced twice at once"
status: draft
illustrates:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
given:
  - "a sync is already running for a connection"
when: "another sync of the same connection is triggered"
then:
  - "the second sync does not run concurrently with the first"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization: Concurrent sync for the same connection is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that syncs of one connection never overlap.

## Boundaries

The openspec allows the second request to be either rejected or deferred; this example does not choose between them.
