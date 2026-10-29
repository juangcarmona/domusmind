---
id: FR-FINANCES-REVIEW-SPENDING
type: functional-requirement
title: Review expected commitments and actual spending
status: draft
derived-from:
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
  - BR-FINANCES-CURRENCY-SAFE-SUMMARIES
verification:
  - scenario: "A member filters actual expense history by period and approved category/context and sees only authorized records and accurate same-currency totals."
  - scenario: "Upcoming expected commitment amounts are shown separately from actual expenses and payment outcomes."
  - scenario: "When more than one currency is present, the product groups totals by currency and performs no implicit conversion."
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
---
## Requirement

The product MUST present upcoming commitments separately from actual expense history and allow authorized members to review expenses by period and supported household categories or contextual references. Summaries MUST preserve currency and distinguish expected, actual and paid amounts.

## Rationale

Useful household spending insight requires transparent totals, not an accounting dashboard or an unexplained aggregate.
