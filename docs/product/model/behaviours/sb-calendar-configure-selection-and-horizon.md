---
id: SB-CALENDAR-CONFIGURE-SELECTION-AND-HORIZON
type: structured-behaviour
title: "A member selects calendars and sets a horizon"
status: draft
illustrates:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
given:
  - "an active external calendar connection exists"
when: "the member selects one or more provider calendars and a supported horizon"
then:
  - "the connection records the selected calendars and the horizon"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration: Member selects calendars and sets a sync horizon)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that the person controls which calendars and how far ahead.

## Boundaries

None.
