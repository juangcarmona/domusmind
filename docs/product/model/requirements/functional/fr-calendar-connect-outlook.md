---
id: FR-CALENDAR-CONNECT-OUTLOOK
type: functional-requirement
title: "Connect an Outlook account for read-only ingestion"
status: draft
derived-from:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "BR-CALENDAR-ONE-CONNECTION-PER-ACCOUNT"
  - "BR-CALENDAR-OUTLOOK-REQUIRED-ACCESS"
  - "BR-CALENDAR-SUPPORTED-SYNC-HORIZONS"
  - "BR-CALENDAR-DEFAULT-REFRESH-HOURLY"
verification:
  - scenario-ref: "SB-CALENDAR-CONNECT-OUTLOOK"
  - scenario-ref: "SB-CALENDAR-CONNECT-DUPLICATE-ACCOUNT"
  - scenario-ref: "SB-CALENDAR-CONNECT-MISSING-ACCESS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (Outlook Account Connection); docs/_legacy/06_interfaces/external-calendar-api.md (Connect Outlook account); src/backend/DomusMind.Domain/Calendar/ExternalConnections/ExternalCalendarConnection.cs (Connect)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a household member connect a Microsoft Outlook account to their own DomusMind identity for read-only calendar ingestion, only when calendar read and continued access are granted and the member has no active connection to that account. A new connection MUST start pending its initial sync with a horizon of one day back to 90 days forward and scheduled refresh on every 60 minutes, and MUST NOT create plans (observed: calendar spec).

## Rationale

People's commitments often live in Outlook; seeing them in their Agenda avoids double entry (inferred).
