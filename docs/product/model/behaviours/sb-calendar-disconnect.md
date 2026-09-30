---
id: SB-CALENDAR-DISCONNECT
type: structured-behaviour
title: "Disconnecting removes the connection and its entries"
status: draft
illustrates:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-DISCONNECT-IS-LOCAL"
  - "BR-CALENDAR-EXTERNAL-ENTRIES-NEVER-BECOME-PLANS"
given:
  - "an active external calendar connection exists"
when: "the member disconnects it"
then:
  - "the connection is removed"
  - "none of its imported entries remain active"
  - "those entries no longer appear in the Agenda"
  - "no plan is changed"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-ENTRY"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Disconnection: Member disconnects a connection)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that disconnection is immediate and leaves household plans alone.

## Boundaries

Does not assert anything about the provider account, which is never touched.
