---
id: BR-ASSET-RECURRENCE-DUE-STATE
type: business-rule
title: Asset obligations retain one pending due cycle
status: active
uses-terms:
  - TERM-ASSET-OBLIGATION
  - TERM-ASSET-OCCURRENCE
---

## Rule

A recurring Asset Obligation exposes one pending due cycle on a calendar date in the configured household timezone, without an exact due time in the first release. A member's device timezone and the server timezone do not change that date. If its date passes without an explicit outcome, that cycle remains overdue; the product does not accumulate new pending cycles or mark missed cycles as skipped automatically. Once the member completes or explicitly skips it, the outcome is recorded and the next due state is calculated from the chosen rule. For a completion-relative rule, an explicit skip advances from the skipped cycle's due date; a completion advances from the actual completion date. Rescheduling moves only the selected pending date and leaves the recurrence rule unchanged. For a monthly rule anchored to the last day of a month, the due date is the last valid day of each month.

## Rationale

The household sees one actionable overdue responsibility without invented outcomes or duplicate reminders. Completion-relative work follows when the work was actually done.

## Examples

- A monthly obligation due on 31 January is due on 28 February in a non-leap year and 31 March; the last-day anchor remains intact.
- If a monthly inspection remains unresolved beyond the next month, it still appears as one overdue pending item until explicitly completed, skipped or rescheduled. Skipping records that decision and calculates the next due state.
- Moving one appointment's due date does not change the obligation's monthly last-day rule.
- A service due in January but completed in March with a twelve-month completion-relative rule is next due twelve months after the March completion date.
- A service with a twelve-month completion-relative rule that is explicitly skipped on its January due date is next due twelve months after that skipped due date, regardless of when the member presses Skip.

## Exceptions

The household timezone is configured by a manager through Household Settings. Other calendar anchors and behavior when that timezone changes need review before implementation. Usage-meter recurrence is outside the initial scope.
