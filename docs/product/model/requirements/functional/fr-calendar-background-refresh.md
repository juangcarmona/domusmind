---
id: FR-CALENDAR-BACKGROUND-REFRESH
type: functional-requirement
title: "Refresh stale external calendars in the background"
status: draft
derived-from:
  - "UC-CALENDAR-REFRESH-EXTERNAL-CALENDARS"
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
  - "BR-CALENDAR-REFRESH-FAILURES-ISOLATED"
  - "BR-CALENDAR-NO-CONCURRENT-SYNC"
verification:
  - scenario-ref: "SB-CALENDAR-REFRESH-STALE"
  - scenario-ref: "SB-CALENDAR-REFRESH-SKIP-FRESH"
  - scenario-ref: "SB-CALENDAR-CATCH-UP-ON-AGENDA-OPEN"
  - scenario: "One connection's background refresh fails; every other stale connection is still refreshed in the same pass (observed: calendar spec, Background Feed Refresh)."
  - scenario: "A member signs in while one of their connections is stale; a catch-up sync is triggered for it (observed: calendar spec, Background Feed Refresh)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "openspec/specs/calendar/spec.md (Background Feed Refresh); docs/_legacy/04_contexts/calendar.md (Sync Rule); interview: product owner decision Q-0047 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST automatically refresh stale external calendar connections in the background, on each connection's own refresh interval with a 60-minute default (decided: Q-0047), and MUST also trigger a catch-up sync when a member signs in or opens their Member-scope Agenda with a stale connection. A connection MUST never sync concurrently with itself, one connection's failure MUST NOT prevent others from refreshing, and refresh MUST NOT create plans (observed: calendar spec).

Undecided (Q-0047): the allowed interval values and the staleness threshold that triggers catch-up are decided later.

## Rationale

Keeps the Agenda current without manual effort (observed: calendar spec).
