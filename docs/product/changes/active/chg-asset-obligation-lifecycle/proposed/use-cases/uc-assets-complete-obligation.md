---
id: UC-ASSETS-COMPLETE-OBLIGATION
type: use-case
title: Record an obligation outcome and asset history
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
- BR-ASSET-OCCURRENCE-HISTORY
- BR-ASSET-RECURRENCE-DUE-STATE
- BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
- BR-ASSET-OBLIGATION-LIFECYCLE
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

The member is authorized for an Active Asset. The obligation is Active and the addressed occurrence is its current Pending cycle. An exact retry of an earlier accepted operation returns its prior outcome without a new transition.

## Main Flow

1. Member records completion date and a concise outcome; may add provider, actual cost, notes and authorized supporting document.
2. System records one durable occurrence outcome and linked Asset Record, shown as a single history entry.
3. For recurring obligations, system calculates the next due state using the rule. Completion-relative recurrence starts from the actual completion date; a monthly last-day calendar rule stays on the last valid day of each month.
4. A one-time obligation becomes Finished after completion or skip; a recurring obligation stays Active with one next Pending cycle.
5. Agenda reflects the updated actionable projection without altering any related Task or Calendar Plan.

## Alternative Flows

- Member explicitly skips the pending cycle; the system records the omission and calculates the next due state under the rule. A completion-relative rule advances from the skipped cycle's due date.
- Member reschedules only the pending cycle; the system retains the original recurrence rule for future cycles.
- Member records unscheduled maintenance directly as an Asset Record without an obligation.
- Member corrects a mistaken completion through a traceable correction linked to the original outcome; the prior history remains visible.

## Failure Conditions

- A new action addresses a resolved or obsolete cycle, or the obligation is Suspended, Cancelled or Finished. It is refused without mutation. An exact idempotent retry returns its prior result without duplicating history or changing a later cycle.
- The member lacks authorization or required input is invalid.

## Postconditions

History remains durable and discoverable; the obligation is advanced, remains due, suspended or cancelled according to the chosen outcome.
