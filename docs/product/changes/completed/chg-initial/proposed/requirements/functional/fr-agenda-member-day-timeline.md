---
id: FR-AGENDA-MEMBER-DAY-TIMELINE
type: functional-requirement
title: "Show a person's day as a timeline"
status: draft
derived-from:
  - "UC-AGENDA-VIEW-MEMBER-AGENDA"
verification:
  - scenario: "Ana has a 09:00-10:00 dentist plan, an all-day school trip and two tasks: the timeline shows the dentist as a one-hour block, the trip in the all-day lane and the tasks in the non-timed section (observed: agenda.md, Member + Day = Timeline)."
uses-terms:
  - "TERM-AGENDA"
  - "TERM-AGENDA-SCOPE"
  - "TERM-AGENDA-MODE"
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/00_product/surfaces/agenda.md (Member + Day = Timeline)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

In Member scope and Day mode the product MUST show an hour timeline from the earliest relevant time to the end of day (at least 06:00 to 23:00), with timed plans as blocks by start and duration, an all-day lane above, and tasks, routines and projected list items (by importance then due date) in a compact non-timed section (observed: agenda.md).

## Rationale

Makes one person's gaps, conflicts and load visible (observed: agenda.md).
