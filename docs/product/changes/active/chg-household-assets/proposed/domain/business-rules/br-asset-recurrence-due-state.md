---
id: BR-ASSET-RECURRENCE-DUE-STATE
type: business-rule
title: Asset obligations retain one pending due cycle
status: draft
applies-to:
  - UC-ASSETS-MANAGE-OBLIGATION
  - UC-ASSETS-COMPLETE-OBLIGATION
uses-terms:
  - TERM-ASSET-OBLIGATION
  - TERM-ASSET-OCCURRENCE
---

## Rule

A recurring Asset Obligation exposes one pending due cycle on a household-local calendar date, without an exact due time in the first release. If its date passes without an explicit outcome, that cycle remains overdue; the product does not accumulate new pending cycles or mark missed cycles as skipped automatically. Once the member completes or explicitly skips it, the outcome is recorded and the next due state is calculated from the chosen rule. Rescheduling moves only the selected pending date and leaves the recurrence rule unchanged. For a monthly rule anchored to the last day of a month, the due date is the last valid day of each month. A completion-relative rule calculates its next due date from the actual completion date.

## Rationale

The household sees one actionable overdue responsibility without invented outcomes or duplicate reminders. Completion-relative work follows when the work was actually done.

## Examples

- A monthly obligation due on 31 January is due on 28 February in a non-leap year and 31 March; the last-day anchor remains intact.
- If a monthly inspection remains unresolved beyond the next month, it still appears as one overdue pending item until explicitly completed, skipped or rescheduled. Skipping records that decision and calculates the next due state.
- Moving one appointment's due date does not change the obligation's monthly last-day rule.
- A service due in January but completed in March with a twelve-month completion-relative rule is next due twelve months after the March completion date.

## Exceptions

How a household timezone is selected, other calendar anchors and the next-due anchor after skipping a completion-relative cycle remain open product questions. Usage-meter recurrence is outside the initial scope.
