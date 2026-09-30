---
id: SB-CALENDAR-UNSUPPORTED-HORIZON
type: structured-behaviour
title: "An unsupported horizon is rejected"
status: draft
illustrates:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-SUPPORTED-SYNC-HORIZONS"
given:
  - "a horizon value other than 30, 90, 180 or 365 days"
when: "the member tries to save the configuration"
then:
  - "the configuration is rejected"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration: Unsupported horizon value is rejected)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes the four supported horizons.

## Boundaries

None.
