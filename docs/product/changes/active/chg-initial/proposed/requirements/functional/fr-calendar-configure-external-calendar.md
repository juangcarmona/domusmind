---
id: FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR
type: functional-requirement
title: "Choose calendars and horizon for a connection"
status: draft
derived-from:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-SUPPORTED-SYNC-HORIZONS"
  - "BR-CALENDAR-HORIZON-CHANGE-RELOADS"
  - "BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT"
verification:
  - scenario-ref: "SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON"
  - scenario-ref: "SB-CALENDAR-DESELECT-FEED"
  - scenario-ref: "SB-CALENDAR-HORIZON-CHANGE-RELOADS"
  - scenario-ref: "SB-CALENDAR-UNSUPPORTED-HORIZON"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration); docs/_legacy/06_interfaces/external-calendar-api.md (Configure connection); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Configure)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a member choose which provider calendars of their connection are imported and a forward horizon of 30, 90, 180 or 365 days. Changing the selection or horizon MUST reload the affected calendars; deselected calendars MUST stop showing entries and lose their sync state; no configuration change may create plans (observed: calendar spec). The member SHOULD also be able to turn scheduled refresh on or off and set its interval (observed: external-calendar-api.md; medium confidence).

## Rationale

Only the person knows which of their calendars are relevant to household life (inferred).
