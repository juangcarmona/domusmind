---
id: TERM-AGENDA-MODE
type: domain-term
title: "Agenda Mode"
status: draft
defined-in: "BC-CALENDAR"
synonyms:
  - "Time mode"
  - "Day view"
  - "Week view"
  - "Month view"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Time Modes); docs/_legacy/03_domain/ubiquitous-language.md (Day View, Week View)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Definition

The time window the Agenda shows: **Day** (one day), **Week** (seven days starting on the household's configured first day of week) or **Month** (a calendar grid of the month) (observed: agenda.md, Time Modes).

Day is presented as a **board** in Household scope (one compact row per person) and as an hour-positioned **timeline** in Member scope (observed: agenda.md, Day).

## Distinguish From

- **Agenda scope**: whose picture is shown, independent of the window (observed: agenda.md).
- **Sync horizon**: the window of external calendar data held locally; the Agenda mode window only filters what is displayed (inferred from agenda.md, External calendar entry rules).

## Usage

Day is the default operational view, Week the default coordination view when Day is not enough, Month a navigation and load-awareness view that is never the default and is not a primary editing surface (observed: agenda.md, Time Modes, Anti-Patterns).
