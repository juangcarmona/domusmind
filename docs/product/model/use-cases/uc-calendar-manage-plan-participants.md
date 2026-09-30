---
id: UC-CALENDAR-MANAGE-PLAN-PARTICIPANTS
type: use-case
title: "Add or remove plan participants"
status: draft
primary-actor: "ACT-HOUSEHOLD-MEMBER"
supporting-actors: []
bounded-context: "BC-CALENDAR"
governed-by:
  - "BR-CALENDAR-PARTICIPANT-IN-HOUSEHOLD"
  - "BR-CALENDAR-PARTICIPANT-UNIQUE"
  - "BR-CALENDAR-CANCELLED-PLAN-IS-CLOSED"
uses-terms:
  - "TERM-PLAN"
  - "TERM-PLAN-PARTICIPANT"
  - "TERM-MEMBER"
  - "TERM-MEMBER-ROLE"
provenance:
  source: "openspec/specs/calendar/spec.md (Event Participant Management); docs/_legacy/04_contexts/calendar.md (Participant); src/backend/DomusMind.Domain/Calendar/CalendarEvent.cs (AddParticipant, RemoveParticipant); interview: product owner decision Q-0042 (E-0158); interview: product owner decision Q-0043 (E-0158)"
  confidence: "high"
  recovered-from: "documentation"
---

## Goal

Keep a plan's list of who needs to be there accurate (observed: legacy calendar.md, Participant).

## Trigger

Someone joins or drops out of a plan.

## Preconditions

- The plan exists and is scheduled (observed: calendar spec).

## Main Flow

1. The person adds a household member (of any role; pets are members with the Pet role, decided: Q-0042) as a participant.
2. DomusMind checks they belong to the household and are not already taking part.
3. DomusMind adds them to the plan's participants (observed: calendar spec).

## Alternative Flows

- 1a. The person removes a current participant; DomusMind removes only that participant and leaves the plan and other participants unchanged (observed: calendar spec).

## Failure Conditions

- The person is already a participant: rejected (observed: calendar spec).
- The plan is cancelled: adding is rejected (observed: calendar spec).
- Removing someone who is not a participant: rejected (observed: calendar spec).
- The plan is cancelled: removing a participant is rejected (decided: Q-0043); the domain code still allows it today (observed: CalendarEvent.cs).

## Postconditions

- The plan shows in the Member-scope Agenda of each participant (observed: calendar spec, Member Agenda Projection).
