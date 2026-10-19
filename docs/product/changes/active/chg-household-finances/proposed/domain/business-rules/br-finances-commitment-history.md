---
id: BR-FINANCES-COMMITMENT-HISTORY
type: business-rule
title: Commitment changes preserve prior financial terms and outcomes
status: draft
applies-to:
  - UC-FINANCES-MANAGE-COMMITMENTS
  - UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Rule

Changing a commitment's amount, due schedule, renewal or cancellation terms affects future projections only from its effective date. A change does not rewrite recorded expenses, payment outcomes or prior expected amounts. Cancelling a commitment ends future projections and preserves historical facts.

## Rationale

Households need to explain price changes and see what was expected or paid at the time, without maintaining an unbounded materialized future schedule.

## Examples

- An insurance premium increases from the next renewal date; prior due cycles retain their earlier expected amounts.
- Cancelling a streaming subscription removes its future projections but leaves its past paid history.
- Retrying a commitment update with the same request does not duplicate a due cycle or history entry.

## Exceptions

Correction of an incorrectly entered past term requires an explicit, auditable correction policy. Exact occurrence persistence and correction rules remain open for product-owner review.
