---
id: SB-CALENDAR-START-SYNC
type: structured-behaviour
title: A member starts one connection sync
status: active
illustrates:
  - UC-CALENDAR-SYNC-EXTERNAL-CALENDAR
given:
  - A member owns a connected Outlook account.
  - No synchronization holds the connection lease.
when: The member requests Sync now.
then:
  - The connection enters Syncing.
covers-transition:
  lifecycle: LC-EXTERNAL-CALENDAR-CONNECTION
  transition: START-SYNC
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
provenance:
  source: FR-CALENDAR-SHOW-CONNECTION-STATUS; UC-CALENDAR-SYNC-EXTERNAL-CALENDAR; product owner decision Q-0049; src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; src/backend/DomusMind.Application/Features/Calendar/SyncExternalCalendarConnection/SyncExternalCalendarConnectionCommandHandler.cs
  confidence: high
  recovered-from: documentation
---

## Intent

Let a member bring one connection up to date while preserving exclusive sync access.

## Boundaries

The example concerns the local connection and leaves provider data and household Plans unchanged.
