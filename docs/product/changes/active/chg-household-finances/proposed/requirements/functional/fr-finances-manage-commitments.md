---
id: FR-FINANCES-MANAGE-COMMITMENTS
type: functional-requirement
title: Manage recurring household financial commitments
status: draft
derived-from:
  - UC-FINANCES-MANAGE-COMMITMENTS
  - BR-FINANCES-COMMITMENT-HISTORY
verification:
  - scenario: "A member creates a recurring commitment with a supported schedule, currency and fixed or explicitly variable expected amount; the system presents bounded upcoming due dates without materializing an unbounded future."
  - scenario: "Changing price or recurrence from an effective date changes only future projections and preserves prior expected terms and outcomes."
  - scenario: "Cancelling a commitment ends its future projections without deleting recorded expenses or payment history."
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Requirement

An authorized household member MUST be able to create, inspect, update, pause or cancel a recurring Financial Commitment with an expected due rule, currency, amount policy and effective dates. The product MUST preserve historical terms and MUST show projected due occurrences as expected, not as actual spending.

## Rationale

Knowing the next expected bill and how its terms changed reduces household memory load without requiring a linked bank account.
