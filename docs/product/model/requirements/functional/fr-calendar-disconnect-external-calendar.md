---
id: FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
type: functional-requirement
title: "Disconnect an external calendar"
status: draft
derived-from:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-DISCONNECT-IS-LOCAL"
verification:
  - scenario-ref: "SB-CALENDAR-DISCONNECT"
  - scenario-ref: "SB-CALENDAR-DISCONNECT-UNKNOWN"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Disconnection); docs/_legacy/06_interfaces/external-calendar-api.md (Disconnect connection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Requirement

The product MUST let a member disconnect an external calendar connection, removing the connection, its imported entries and the locally held access, with the entries disappearing from the Agenda immediately, without any operation on the provider account and without changing any plan (observed: calendar spec).

## Rationale

People must be able to withdraw their external calendar completely (inferred).
