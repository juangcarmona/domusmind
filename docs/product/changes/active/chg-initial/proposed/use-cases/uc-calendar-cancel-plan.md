---
id: UC-CALENDAR-CANCEL-PLAN
type: use-case
title: "Cancel a plan"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
uses-terms:
  - "TERM-PLAN"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Cancellation); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (Cancel); interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Take a plan out of active planning while keeping it in history (observed: calendar spec).

## Trigger

A plan will no longer happen.

## Preconditions

- The plan exists and is scheduled (observed: calendar spec).

## Main Flow

1. The person cancels the plan.
2. DomusMind marks it cancelled; it is kept in history and is no longer active (observed: calendar spec).

## Alternative Flows

None recorded in the sources.

## Failure Conditions

- The plan is already cancelled: the second cancellation is rejected (observed: calendar spec).

## Postconditions

- The plan is cancelled and nothing about it may change any more: schedule, details, participants and reminders are fixed (observed: calendar spec; decided: Q-0043).
- Whether and how a cancelled plan still shows in the Agenda is not specified (inferred gap).
