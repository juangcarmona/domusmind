---
id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
type: functional-requirement
title: "Sync an external calendar on demand"
status: draft
derived-from:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-READ-ONLY"
verification:
  - scenario-ref: "SB-CALENDAR-MANUAL-SYNC"
  - scenario-ref: "SB-CALENDAR-SYNC-INVALID-STATE"
  - scenario-ref: "SB-CALENDAR-SYNC-CONCURRENT"
  - scenario-ref: "SB-CALENDAR-SYNC-NO-FEEDS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Synchronization); docs/_legacy/04_contexts/calendar.md (Recovery Rule)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a member manually synchronize one of their connections, importing new, updated and deleted occurrences from every selected calendar within the horizon, incrementally when valid sync state exists and otherwise as a fresh bounded load. Two syncs of one connection MUST NOT run concurrently, and imported entries MUST remain read-only (observed: calendar spec).

## Rationale

People sometimes need their Agenda current right now, not at the next hourly refresh (inferred).
