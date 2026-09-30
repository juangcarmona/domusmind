---
id: UC-AGENDA-NAVIGATE
type: use-case
title: "Move through the Agenda by scope, mode and date"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-AGENDA-DEFAULT-ENTRY-STATE"
  - "BR-AGENDA-SCOPE-SWITCH-KEEPS-CONTEXT"
  - "BR-AGENDA-WEEK-STARTS-ON-HOUSEHOLD-FIRST-DAY"
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Header, Default Entry State, Entry Points, Mobile Behavior)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Goal

Reach the right window of the right person's picture in one or two moves (observed: agenda.md, Header).

## Trigger

The person uses the scope selector, the mode toggle, previous/today/next, taps a day in Month, or follows a link to a specific state (observed: agenda.md).

## Preconditions

- The Agenda is open.

## Main Flow

1. The person picks Household or a person in the scope selector; mode and date are kept.
2. The person picks Day, Week or Month; scope is kept.
3. The person moves to the previous or next window, or back to today; the header shows the day, date or range (observed: agenda.md, Header).

## Alternative Flows

- 3a. In Month, tapping a day opens Day mode for that date (observed: agenda.md, Month).
- 3b. On mobile, dates are swiped on the canvas and in Week a date strip switches to Day for the tapped date (observed: agenda.md, Mobile Week).
- 3c. A link opens a specific scope, mode and date directly, for example one person's week (observed: agenda.md, Entry Points).

## Failure Conditions

- Not specified in the sources.

## Postconditions

- The Agenda shows the requested scope, mode and date.
