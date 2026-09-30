---
id: BR-CALENDAR-MEMBER-MANAGES-OWN-CONNECTIONS
type: business-rule
title: "People manage only their own external calendar connections; managers see others' status"
status: draft
applies-to:
  - "UC-CALENDAR-CONNECT-OUTLOOK"
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
  - "UC-CALENDAR-VIEW-EXTERNAL-CALENDAR-CONNECTIONS"
  - "UC-CALENDAR-REVIEW-HOUSEHOLD-CONNECTION-STATUS"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-MANAGER"
provenance:
  source: "docs/_legacy/06_interfaces/external-calendar-api.md (Authorization and Scope); openspec/specs/calendar/spec.md (Outlook Account Connection, member-scoped); interview: product owner decision Q-0048 (E-0158)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Rule

In Phase 1 each person manages their own provider credentials and connections (observed: external-calendar-api.md, Authorization and Scope). A household manager sees a summary of other people's external calendar connections, showing their status but not their content, and cannot connect, configure, sync or disconnect them (decided: Q-0048).

## Rationale

Provider accounts are personal and often work accounts (inferred).

## Examples

- Ana cannot connect, configure or disconnect Leo's Outlook (observed: external-calendar-api.md, 403 cases).
- Ana, a household manager, sees that Leo's Outlook connection needs attention, but not Leo's calendars or entries (decided: Q-0048).

## Exceptions

The status summary a household manager sees of others' connections (decided: Q-0048). Which fields besides status the summary carries, and where it is shown, are not specified.
