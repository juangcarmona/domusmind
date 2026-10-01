---
id: SB-CALENDAR-HORIZON-CHANGE-RELOADS
type: structured-behaviour
title: "Changing the horizon triggers a fresh load"
status: draft
illustrates:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
given:
  - "an active connection has stored sync state"
when: "the member changes the horizon to a different supported value"
then:
  - "the stored incremental sync state of affected calendars is discarded"
  - "the next sync loads the new window afresh"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration: Horizon change triggers rehydration)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that the horizon is part of the sync state's identity.

## Boundaries

None.
