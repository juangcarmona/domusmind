---
id: BR-CALENDAR-COMPLETED-PLAN-SCHEDULE-FIXED
type: business-rule
title: "A completed plan cannot change its schedule"
status: draft
applies-to:
  - "UC-CALENDAR-COMPLETE-PLAN"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Lifecycle, Suggested future commands: CompleteEvent); openspec/specs/calendar/spec.md (Note 2)"
  confidence: "low"
  recovered-from: "documentation"
---

## Rule

Planned: future (version not stated). Once plan completion exists, a completed plan cannot change its schedule (observed: legacy calendar.md, Lifecycle).

## Rationale

Not stated; the openspec records it only as an invariant implied by a future completion state (observed: calendar spec, Note 2).

## Examples

- None recorded in the sources.

## Exceptions

Plans have no completed state today: the domain code only knows scheduled and cancelled (observed: EventStatus.cs).
