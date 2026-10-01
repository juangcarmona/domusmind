---
id: UC-CALENDAR-MANAGE-RECURRING-PLANS
type: use-case
title: "Manage recurring plans"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-RECURRING-PLAN-HAS-RULE"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "docs/_legacy/04_contexts/calendar.md (Schedule Semantics; Suggested future commands: CreateRecurringEvent, SkipOccurrence, MoveOccurrence); openspec/specs/calendar/spec.md (Note 1); interview: product owner decision Q-0044 (E-0158)"
  confidence: "low"
  recovered-from: "documentation"
---

## Goal

Planned: V2 (decided: Q-0044). Represent a repeated time commitment once, such as football practice every Tuesday, and adjust single occurrences (observed: legacy calendar.md).

## Trigger

A person has a commitment that repeats on a pattern.

## Preconditions

- Not specified in the sources.

## Main Flow

1. The person schedules a plan with a recurrence rule.
2. DomusMind shows each occurrence on its date (inferred).

## Alternative Flows

- 2a. The person skips one occurrence (observed: legacy calendar.md, SkipOccurrence).
- 2b. The person moves one occurrence (observed: legacy calendar.md, MoveOccurrence).

## Failure Conditions

- A recurring plan without a recurrence rule is rejected (observed: legacy calendar.md, Schedule invariant).

## Postconditions

- The recurring plan and its occurrences appear in the Agenda (inferred).

No feature spec and no domain code cover recurrence today (observed: calendar spec, Note 1; inferred from src/backend/DomusMind.Domain/Calendar).
