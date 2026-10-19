---
id: UC-ASSETS-COMPLETE-OBLIGATION
type: use-case
title: Record an obligation outcome and asset history
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
  - BR-ASSET-OCCURRENCE-HISTORY
  - BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
uses-terms:
  - TERM-ASSET-OBLIGATION
  - TERM-ASSET-OCCURRENCE
  - TERM-ASSET-RECORD
---
## Goal

Preserve what happened for a due requirement and determine the next due state when applicable.

## Trigger

A member has completed, skipped or needs to reschedule a due occurrence.

## Preconditions

The member is authorized for the Asset and the obligation is active or has a resolvable due occurrence.

## Main Flow

1. Member records completion date and a concise outcome; may add provider, actual cost, notes and authorized supporting document.
2. System records one durable occurrence outcome and linked Asset Record.
3. For recurring obligations, system calculates the next due state using the agreed recurrence anchor and calendar rules.
4. Agenda reflects the updated projection without altering any related Task or Calendar Plan.

## Alternative Flows

- Member explicitly skips or reschedules an occurrence; the reason and effect on future recurrence are defined by product rules.
- Member records unscheduled maintenance directly as an Asset Record without an obligation.

## Failure Conditions

- The occurrence was already resolved; an idempotent retry returns the prior result rather than duplicating history.
- The member lacks authorization or required input is invalid.

## Postconditions

History remains durable and discoverable; the obligation is advanced, remains due, suspended or cancelled according to the chosen outcome.
