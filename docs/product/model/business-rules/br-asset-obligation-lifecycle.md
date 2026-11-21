---
id: BR-ASSET-OBLIGATION-LIFECYCLE
type: business-rule
title: Asset obligation actions follow explicit lifecycle states
status: active
uses-terms:
- TERM-ASSET
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Rule

An obligation starts Active with exactly one Pending due cycle. Due and overdue are calculated from that pending household-local date; passage of time alone does not resolve or replace the cycle.

For an authorized member and an Active Asset, the allowed actions are:

| Obligation state | Allowed lifecycle actions | Actions refused |
| --- | --- | --- |
| Active with a Pending cycle | Suspend, cancel, complete, skip and reschedule the pending cycle | Reactivate |
| Suspended | Reactivate or cancel | Complete, skip, reschedule and another suspension |
| Cancelled | Consult retained information and history | Reactivate, suspend, cancel again or record a new cycle outcome |
| Finished | Consult retained information and history | Reactivate, suspend, cancel or record a new cycle outcome |

Suspension preserves the same pending cycle, date, recurrence rule and history. It contributes no actionable upcoming or overdue item while suspended. Explicit reactivation returns the same obligation to Active with the same pending date; if that date is past, the cycle is overdue. No outcomes or replacement cycles are invented during suspension.

Cancellation is terminal for that obligation. Its pending cycle becomes Cancelled without being marked Completed or Skipped. Prior history stays readable. A new requirement is represented by a new obligation rather than reopening the cancelled one.

Completing or skipping a one-time obligation resolves its only cycle and makes the obligation Finished. For a recurring obligation, completion or skip resolves the current cycle, keeps the obligation Active and calculates exactly one next pending cycle using BR-ASSET-RECURRENCE-DUE-STATE. Completed, Skipped and Cancelled cycles are terminal: ordinary actions cannot overwrite them. Rescheduling is a Pending-to-Pending change to the same cycle, preserves its identity and recurrence rule, and retains the explicit date change in bounded history.

An exact retry of an already accepted operation returns its prior outcome without applying it to a later cycle or duplicating history. A conflicting new action against an obsolete or resolved cycle is refused and changes neither history nor the current cycle. Competing actions cannot produce two outcomes for the same cycle or silently lose an accepted outcome.

An Archived Asset permits consultation of its obligations and history, but refuses new obligation definitions and obligation lifecycle mutations. Archive and restore do not themselves suspend, cancel, resolve or reactivate an obligation and do not change its pending local date. An obligation that is still Active becomes actionable on Asset restoration using that same pending date; any resulting overdue state is retained. An obligation that was Suspended remains Suspended after Asset restoration.

## Rationale

The household can distinguish a temporary pause, a stopped requirement and a resolved one-time requirement. Explicit terminal outcomes preserve history without allowing a later request to change what already happened.

## Examples

- An obligation suspended with a January due date is reactivated in March with that same overdue January cycle.
- Cancelling a pending inspection closes its cycle as Cancelled and preserves earlier completed inspections.
- A completed one-time inspection has a Finished obligation and no new due cycle.
- A recurring service completed twice with the same operation keeps one outcome and one next pending cycle.
- Restoring an Asset does not reactivate a separately Suspended obligation.

## Exceptions

Traceable corrections and their effect on future due state remain separate Asset Record work; an ordinary outcome command is never a correction. Other calendar anchors, household timezone-change behavior, automatic resumption, sale-specific workflows and definitive deletion are not selected here.
