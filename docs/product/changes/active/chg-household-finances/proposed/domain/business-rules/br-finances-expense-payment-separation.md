---
id: BR-FINANCES-EXPENSE-PAYMENT-SEPARATION
type: business-rule
title: Expected commitments, actual expenses and payment outcomes remain distinct
status: draft
applies-to:
  - UC-FINANCES-MANAGE-COMMITMENTS
  - UC-FINANCES-RECORD-COST
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
---
## Rule

A commitment is an expectation, an Expense is a cost incurred, and a Payment Record is a settlement fact. Recording one must not silently create a second chargeable cost representation. The accepted model must make it possible to see upcoming amounts, actual expense totals and payment status without double-counting one real-world cost.

## Rationale

Expected, incurred and settled amounts answer different household questions. Collapsing them loses meaning; duplicating them distorts summaries.

## Examples

- An upcoming electricity bill is visible as a commitment due occurrence without increasing actual spending.
- Recording a boiler inspection creates one actual expense; it need not create a recurring commitment.
- Marking a due cycle paid preserves its expected amount and records actual payment information under the approved expense/payment relationship.

## Exceptions

Split payments, partial settlements, refunds, credits and non-expense transfers are not assumed for MVP. Their impact on totals and status is an owner decision before implementation.
