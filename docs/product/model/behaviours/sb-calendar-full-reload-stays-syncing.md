---
id: SB-CALENDAR-FULL-RELOAD-STAYS-SYNCING
type: structured-behaviour
title: A full calendar reload remains part of Syncing
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - A connection is Syncing.
  - A full reload is required because its incremental sync state is invalid.
when: The connection loads the selected calendars for its configured horizon.
then:
  - The connection remains Syncing while the full reload runs.
  - No separate Rehydrating product state is shown.
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
  - TERM-EXTERNAL-CALENDAR-SYNC
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Keep one clear progress state for incremental synchronization and a full reload.

## Boundaries

The eventual success or failure follows the existing connection outcome transitions.
