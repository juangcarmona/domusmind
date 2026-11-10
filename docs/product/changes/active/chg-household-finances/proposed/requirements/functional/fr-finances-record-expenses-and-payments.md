---
id: FR-FINANCES-RECORD-EXPENSES-AND-PAYMENTS
type: functional-requirement
title: Record actual expenses and commitment payment outcomes
status: draft
derived-from:
  - UC-FINANCES-RECORD-COST
  - BR-FINANCES-EXPENSE-PAYMENT-SEPARATION
verification:
  - scenario: "A member records an actual cost without a recurring commitment and it appears once in the authorized expense history."
  - scenario: "A member records payment against a due occurrence; expected and actual amounts remain distinct, the state updates once on retry, and summaries do not double-count the economic cost."
  - scenario: "A variable bill may be paid for an amount different from the expected amount without rewriting the expected terms."
uses-terms:
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Requirement

An authorized member MUST be able to record actual household Expenses with amount, currency, date, description and optional category/context attribution, and record an actual payment outcome against a Financial Due Occurrence. A record MUST distinguish expected amounts from actual cost/settlement and MUST NOT create duplicate spending totals for the same economic event.

## Rationale

Households need history of what a service or activity cost and whether an expected payment was made; these facts are related but are not interchangeable.
