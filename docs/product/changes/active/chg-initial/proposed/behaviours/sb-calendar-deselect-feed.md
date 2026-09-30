---
id: SB-CALENDAR-DESELECT-FEED
type: structured-behaviour
title: "A deselected calendar stops appearing"
status: draft
illustrates:
  - "UC-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR"
  - "BR-CALENDAR-ONLY-SELECTED-FEEDS-IMPORT"
given:
  - "a provider calendar is selected and its entries appear"
when: "the member deselects that calendar"
then:
  - "its stored entries stop appearing in the Agenda"
  - "its sync state is cleared"
uses-terms:
  - "TERM-EXTERNAL-CALENDAR-CONNECTION"
  - "TERM-EXTERNAL-CALENDAR-FEED"
  - "TERM-SYNC-HORIZON"
provenance:
  source: "openspec/specs/calendar/spec.md (External Calendar Configuration: Deselected feed stops projecting)"
  confidence: "high"
  recovered-from: "documentation"
---

## Intent

Establishes that deselection takes effect on what is shown and on sync state.

## Boundaries

None.
