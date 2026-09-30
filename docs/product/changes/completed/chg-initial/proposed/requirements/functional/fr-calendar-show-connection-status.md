---
id: FR-CALENDAR-SHOW-CONNECTION-STATUS
type: functional-requirement
title: "Show a member their connections and sync health"
status: draft
derived-from:
  - "UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS"
  - "BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS"
verification:
  - scenario: "A member whose Outlook authorization expired opens their connections and sees that connection flagged as needing re-authorization with the last error, while their other connection shows its last successful sync (observed: external-calendar-api.md; ExternalCalendarConnectionStatus.cs)."
  - scenario: "A member opens one connection and sees its selected and available provider calendars with each calendar's sync window (observed: external-calendar-api.md, Get connection detail)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnectionStatus.cs; docs/_legacy/06_interfaces/external-calendar-api.md (List member connections, Get connection detail); docs/_legacy/06_interfaces/external-calendar-contract-model-catalog.md (Status values); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs; interview: product owner decision Q-0049 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST show a member each of their external calendar connections with provider, account, label, number of selected calendars, horizon, refresh settings, last successful sync, whether a sync is running, its status and the last error, and for one connection its selected and available calendars (observed: external-calendar-api.md). The statuses shown MUST be the connection statuses of the domain code: pending initial sync, healthy, syncing, needs attention, partial failure, failed, authorisation expired, rehydrating and disconnected (observed: ExternalCalendarConnectionStatus.cs; decided: Q-0049). The legacy catalog's shorter set is superseded (observed: external-calendar-contract-model-catalog.md; decided: Q-0049).

## Rationale

People need to know whether what they see from Outlook is current (inferred).
