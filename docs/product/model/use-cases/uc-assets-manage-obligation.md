---
id: UC-ASSETS-MANAGE-OBLIGATION
type: use-case
title: Define an asset obligation
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
  - BR-ASSET-RECURRENCE-DUE-STATE
uses-terms:
  - TERM-ASSET
  - TERM-ASSET-OBLIGATION
---
## Goal

Remember a persistent requirement associated with an Asset and see when it is due.

## Trigger

A member identifies a one-time or recurring inspection, service, renewal or other obligation.

## Preconditions

The member is authorized to manage the household Asset.

## Main Flow

1. Member chooses a requirement and its due rule.
2. Member sets a one-time household-local date or supported recurring calendar/completion-relative rule, and optional, separate responsible member and Area plus descriptive provider information. The first release does not ask for an exact due time.
3. System validates the local date and recurrence semantics and saves the obligation. A monthly last-day rule stays on the last valid day of each month.
4. Agenda can show one pending next-due/overdue cycle as a distinct projection. An unresolved cycle remains overdue until a member explicitly resolves it.

## Alternative Flows

- Member may suspend or cancel an obligation while preserving prior history.
- Usage-meter recurrence is not available in the initial scope.

## Failure Conditions

- Date or recurrence input is ambiguous or unsupported.
- Asset or responsibility reference is outside the household or no longer valid.

## Postconditions

The requirement remains identifiable independently of any Task, Routine or Calendar Plan.
