---
id: UC-CALENDAR-COMPLETE-PLAN
type: use-case
title: "Complete a plan"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-COMPLETED-PLAN-SCHEDULE-FIXED"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Suggested future commands: CompleteEvent; Lifecycle); openspec/specs/calendar/spec.md (Note 2)"
  confidence: "low"
  recovered-from: "documentation"
---

## Goal

Planned: future (version not stated). Mark a plan as having happened (inferred from legacy calendar.md, CompleteEvent).

## Trigger

Not specified in the sources.

## Preconditions

- Not specified in the sources.

## Main Flow

1. The person marks a plan as completed.
2. DomusMind records it as completed; its schedule can no longer change (observed: legacy calendar.md, Lifecycle).

## Alternative Flows

None recorded in the sources.

## Failure Conditions

Not specified in the sources.

## Postconditions

- The plan is completed. The lifecycle is unspecified (observed: calendar spec, Note 2); plans have no completed state today (observed: EventStatus.cs).
