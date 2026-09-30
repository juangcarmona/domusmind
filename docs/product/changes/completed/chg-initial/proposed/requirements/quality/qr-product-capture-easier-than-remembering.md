---
id: QR-PRODUCT-CAPTURE-EASIER-THAN-REMEMBERING
type: quality-requirement
title: "Capturing household state is easier than remembering it"
status: draft
quality-attribute: "usability"
applies-to:
  - "UC-LISTS-ADD-ITEM"
  - "UC-TASKS-CREATE-TASK"
  - "UC-CALENDAR-SCHEDULE-PLAN"
  - "UC-TASKS-CREATE-ROUTINE"
  - "UC-AGENDA-CREATE-FROM-AGENDA"
  - "UC-LISTS-CREATE-LIST"
verification:
  - scenario: "A person on the Agenda adds a plan with only a title and start time, a task with only a title and a list item with only a name, each without leaving the surface they are on and without filling any other field (inferred from experience.md, Core Experience Principles; surface-system.md, Capture stays local)."
uses-terms:
  - "TERM-PLAN"
  - "TERM-TASK"
  - "TERM-LIST-ITEM"
  - "TERM-ROUTINE"
provenance:
  source: "docs/_legacy/00_product/strategy.md (Differentiators); docs/_legacy/00_product/experience.md (Core Experience Principles); docs/_legacy/00_product/surface-system.md (Interaction Grammar: Capture stays local); openspec/specs/web-app/spec.md (Inspector and Modal Usage)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

Adding or updating household state MUST stay easier than remembering it: capture MUST ask only for the minimum each item needs, MUST happen from the surface the person is on, and every other detail MUST be optional and addable later (observed: docs/_legacy/00_product/strategy.md, Differentiators "Capture must stay easier than remembering, or adoption fails"; docs/_legacy/00_product/experience.md, Core Experience Principles; docs/_legacy/00_product/surface-system.md, Interaction Grammar "Capture stays local").

## Measurement

For each covered capture path, count the fields that must be filled and the navigations needed to finish it. Pass = the required fields are only those the area rules require (list name, item name, task title, plan title and start, routine name and recurrence) and zero navigations away from the current surface (inferred method; the sources state the obligation, not the metric). The Lists area measures its own quick add in more detail (QR-LISTS-FRICTIONLESS-CAPTURE).
