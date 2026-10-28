---
id: UC-ASSETS-MANAGE-OBLIGATION
type: use-case
title: Define an asset obligation
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
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
2. Member sets a one-time date or supported recurring calendar/completion-relative rule, and optional responsible member/Area and descriptive provider information.
3. System validates date, timezone and recurrence semantics and saves the obligation.
4. Agenda can show its next due/overdue state as a distinct projection.

## Alternative Flows

- Member may suspend or cancel an obligation while preserving prior history.
- Usage-meter recurrence is not available in the initial scope.

## Failure Conditions

- Date or recurrence input is ambiguous or unsupported.
- Asset or responsibility reference is outside the household or no longer valid.

## Postconditions

The requirement remains identifiable independently of any Task, Routine or Calendar Plan.
