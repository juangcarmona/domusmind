---
id: FR-CALENDAR-EDIT-PLAN-DETAILS
type: functional-requirement
title: "Edit a plan's title, description and colour"
status: draft
derived-from:
  - "UC-CALENDAR-EDIT-PLAN-DETAILS"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
verification:
  - scenario: "A member renames \"Dentist\" to \"Dentist - Leo\" and adds a description; the plan keeps its time, participants and reminders (observed: CalendarEvent.cs Edit; legacy calendar.md RenameEvent)."
  - scenario: "A member tries to rename a cancelled plan; the change is rejected (observed: CalendarEvent.cs)."
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Commands: RenameEvent); openspec/specs/calendar/spec.md (Note 3); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Edit, Repaint)"
  confidence: "medium"
  recovered-from: "documentation"
---

## Requirement

The product SHOULD let a household member change a scheduled plan's title, description and colour without changing its identity or schedule, and MUST reject such changes on a cancelled plan (observed: CalendarEvent.cs; legacy calendar.md). The openspec leaves renaming unspecified beyond identity stability (observed: calendar spec, Note 3).

## Rationale

Plans are captured quickly and refined later (inferred).
