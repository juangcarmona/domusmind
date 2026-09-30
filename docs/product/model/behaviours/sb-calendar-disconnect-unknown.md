---
id: SB-CALENDAR-DISCONNECT-UNKNOWN
type: structured-behaviour
title: "Disconnecting a non-existent connection is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR"
given:
  - "a connection does not exist"
when: "a disconnection of it is attempted"
then:
  - "the disconnection is rejected"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Disconnection: Disconnecting a non-existent connection is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that only existing connections can be disconnected.

## Boundaries

None.
