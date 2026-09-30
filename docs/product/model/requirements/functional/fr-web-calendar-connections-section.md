---
id: FR-WEB-CALENDAR-CONNECTIONS-SECTION
type: functional-requirement
title: "Manage Outlook connections inline in Settings"
status: draft
derived-from:
  - "UC-WEB-MANAGE-SETTINGS"
  - "UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS"
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
verification:
  - scenario: "A person with two Outlook connections sees both rows with provider, account email, included calendar count, horizon such as \"Next 90 days\", last sync time and status; selecting Sync now on one shows progress inline and then its new last sync time; Sync calendars syncs both and still shows that one of them failed (observed: web-app spec, Outlook Calendar Connection Management; settings.md, Calendar Connections Section)."
  - scenario: "A person with no connection sees a short explanation that imported Outlook items appear read-only in the Agenda and a Connect Outlook action (observed: settings.md, Empty state)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
provenance:
  source: "openspec/specs/web-app/spec.md (Outlook Calendar Connection Management); docs/_legacy/00_product/surfaces/settings.md (Calendar Connections Section, Interaction Rules, Mobile Behavior, Success Criteria); interview: product owner decision Q-0049 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

In Settings / Profile the product MUST let a person connect, configure, sync and disconnect their own Outlook calendars (observed: openspec/specs/web-app/spec.md, Outlook Calendar Connection Management):

- The section header MUST offer Connect Outlook, and Sync calendars when more than one connection exists (observed: docs/_legacy/00_product/surfaces/settings.md, Header actions).
- Each connection row MUST show the provider, account email, included calendar count, sync horizon, last sync time and current status, with Sync now, Edit and Disconnect actions (observed: openspec/specs/web-app/spec.md; docs/_legacy/00_product/surfaces/settings.md, Connection rows).
- After the Outlook sign-in flow the new connection MUST appear and be configurable immediately (calendar selection and horizon) (observed: openspec/specs/web-app/spec.md, Connect).
- Editing MUST happen inline on desktop or in a pushed section on mobile, covering included calendars, the horizon (30 / 90 / 180 / 365 days), scheduled refresh and the last sync and error summary (observed: docs/_legacy/00_product/surfaces/settings.md, Detail / edit state).
- Sync now MUST show progress and its success or failure inline without leaving Settings; Sync calendars MUST show aggregate progress without hiding any single connection's failure (observed: openspec/specs/web-app/spec.md, Sync now, Sync calendars).
- With no connection, the section MUST explain briefly that imported Outlook items appear read-only in the Agenda (observed: docs/_legacy/00_product/surfaces/settings.md, Empty state).

The statuses shown are the connection statuses of the domain code (decided: Q-0049; see FR-CALENDAR-SHOW-CONNECTION-STATUS); the surface spec's labels (Connected, Syncing, Needs attention, Auth expired, Rehydrating) are presentation of that set, and how each code status is labelled is not specified (observed: settings.md).

## Rationale

A person must be able to connect Outlook without confusion and see at a glance whether sync is healthy and what to do about it (observed: settings.md, Success Criteria).
