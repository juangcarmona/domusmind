---
id: FR-CALENDAR-SHOW-CONNECTION-STATUS
type: functional-requirement
title: Show a member their connections and sync health
status: active
derived-from:
  - UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS
  - BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS
verification:
  - scenario: "A member whose Outlook authorization expired opens their connections and sees that connection flagged as needing re-authorization with the last error, while their other connection shows its last successful sync (observed: external-calendar-api.md; ExternalCalendarConnectionStatus.cs)."
  - scenario: "A member opens one connection and sees its selected and available provider calendars with each calendar's sync window (observed: external-calendar-api.md, Get connection detail)."
  - scenario-ref: SB-CALENDAR-SYNC-SUCCESS
  - scenario-ref: SB-CALENDAR-SYNC-PARTIAL-FAILURE
  - scenario-ref: SB-CALENDAR-SYNC-FAILURE
  - scenario-ref: SB-CALENDAR-AUTH-EXPIRED
  - scenario-ref: SB-CALENDAR-FULL-RELOAD-STAYS-SYNCING
uses-terms:
  - TERM-EXTERNAL-CALENDAR-CONNECTION
  - TERM-EXTERNAL-CALENDAR-FEED
  - TERM-SYNC-HORIZON
provenance:
  source: 'src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnectionStatus.cs; docs/_legacy/06_interfaces/external-calendar-api.md (List member connections, Get connection detail); docs/_legacy/06_interfaces/external-calendar-contract-model-catalog.md (Status values); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; interview: product owner decision Q-0049 (E-0158); product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: documentation
---

## Requirement

The product MUST show a member each of their external calendar connections with provider, account, label, number of selected calendars, horizon, refresh settings, last successful sync, whether a sync is running, its status and the last error, and for one connection its selected and available calendars (observed: external-calendar-api.md). The statuses shown MUST be Pending initial sync, Healthy, Syncing, Partial failure, Failed, Authorization expired and Disconnected. Failed and Authorization expired MUST show the failure reason. A full reload MUST remain part of Syncing. Needs attention and Rehydrating are not separate product states; this owner decision supersedes that part of Q-0049 (CHG-STATE-MODEL-COMPLETION).

## Rationale

People need to know whether what they see from Outlook is current (inferred).
