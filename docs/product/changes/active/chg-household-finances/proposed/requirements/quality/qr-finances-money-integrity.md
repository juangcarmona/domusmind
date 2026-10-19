---
id: QR-FINANCES-MONEY-INTEGRITY
type: quality-requirement
title: Financial amounts and summaries preserve currency and precision
status: draft
quality-attribute: integrity
applies-to:
  - BC-HOUSEHOLD-FINANCES
  - UC-FINANCES-RECORD-COST
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
verification:
  - scenario: "Amounts are persisted and calculated without binary floating-point error, display the associated currency, follow that currency's approved precision, and never sum unlike currencies."
  - scenario: "Expected terms, actual expense amounts and payment outcomes remain distinguishable through edits and summary queries."
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
---
## Requirement

The product MUST preserve exact monetary values at the approved currency precision, MUST associate every value with a currency, and MUST NOT silently combine unlike currencies or conflate expected, incurred and settled amounts.

## Measurement

Automated tests cover supported currency precision, large/small values, rounding boundaries, changed expected amounts, mixed-currency groups and expense/payment non-duplication. Pass requires exact persisted values and correct totals for every fixture, with zero implicit currency conversions.
