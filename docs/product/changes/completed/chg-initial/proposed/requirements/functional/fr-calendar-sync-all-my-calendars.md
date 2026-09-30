---
id: FR-CALENDAR-SYNC-ALL-MY-CALENDARS
type: functional-requirement
title: "Sync all of a member's calendars at once"
status: draft
derived-from:
  - "UC-CALENDAR-SYNC-EXTERNAL-CALENDAR"
verification:
  - scenario: "A member with three eligible connections uses \"Sync calendars\": a sync is requested for each of the three and the product acknowledges the request before all have finished (observed: external-calendar-api.md, Sync all connections for one member)."
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-SYNC"
  - "TERM-EXTERNAL-CALENDAR-FEED"
provenance:
  source: "docs/_legacy/06_interfaces/external-calendar-api.md (Sync all connections for one member)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product SHOULD offer a single "Sync calendars" action that requests a sync of each of the member's eligible connections separately and reports how many were accepted and skipped, without waiting for them to finish (observed: external-calendar-api.md).

## Rationale

A member with several connections should not have to sync each one by hand (inferred).
