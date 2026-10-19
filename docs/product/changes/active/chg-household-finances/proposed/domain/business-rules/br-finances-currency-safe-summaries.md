---
id: BR-FINANCES-CURRENCY-SAFE-SUMMARIES
type: business-rule
title: Financial summaries never add amounts with different currencies
status: draft
applies-to:
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
uses-terms:
  - TERM-EXPENSE
  - TERM-FINANCIAL-COMMITMENT
---
## Rule

Every amount is interpreted with an explicit currency and supported precision. A summary may total only like-currency values; the product does not silently convert, round across currencies or present a mixed-currency total as one amount.

## Rationale

An apparently precise household total is misleading if its currency or precision is ambiguous.

## Examples

- Monthly expenses recorded in EUR are totalled in EUR.
- If a household has both EUR and GBP entries, the view shows separate totals or a clearly grouped breakdown.
- A value with more fractional digits than the currency supports is rejected or normalized under an explicit rule, never silently rounded without disclosure.

## Exceptions

Currency conversion and exchange-rate sourcing are out of scope for the initial proposal and require a separate product decision.
