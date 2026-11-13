---
id: SB-CALENDAR-SYNC-PARTIAL-FAILURE
type: structured-behaviour
title: A connection reports a partial feed failure
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - The connection is Syncing.
  - At least one selected feed succeeds and another fails.
when: The synchronization finishes.
then:
  - The connection becomes Partial failure.
  - Successful feed results remain available.
  - The failure is reported.
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: SYNC-PARTIAL-FAILURE
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
provenance:
  source: FR-CALENDAR-SHOW-CONNECTION-STATUS; UC-CALENDAR-SYNC-EXTERNAL-CALENDAR; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

A connection reports a partial feed failure.

## Boundaries

The example concerns the local connection and leaves provider data and household Plans unchanged.
