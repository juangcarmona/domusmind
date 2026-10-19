---
id: UC-FINANCES-MANAGE-COMMITMENTS
type: use-case
title: Manage a household financial commitment
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-FINANCES
governed-by:
  - BR-FINANCES-COMMITMENT-HISTORY
  - BR-FINANCES-EXPENSE-PAYMENT-SEPARATION
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Goal

Maintain a recurring household payment expectation and understand its next due date and expected amount.

## Trigger

A household member learns of a new bill, subscription, premium, fee or renewal, or needs to change or cancel an existing commitment.

## Preconditions

- The member is authenticated, belongs to the household and is authorized to see and manage the selected financial scope.
- Required recurrence, currency and amount policy are known or explicitly marked variable/unknown according to the accepted model.

## Main Flow

1. The member captures the commitment name/type and provider/payee as applicable.
2. The member configures its due recurrence, expected amount policy, currency, effective date and optional renewal/cancellation information.
3. The member optionally associates an Asset, Member, Area or supporting Document through an approved context contract.
4. Finances validates household scope, policy and schedule, then saves the commitment and its effective-dated terms.
5. Finances shows the next bounded set of projected due occurrences and expected amounts.

## Alternative Flows

- A variable bill may have no fixed expected amount or may use an estimate explicitly labelled as expected, not actual.
- A member changes terms; only future projections from the effective date change.
- The member cancels the commitment; future cycles stop while history remains.
- A one-time expense without a recurring commitment is recorded through UC-FINANCES-RECORD-COST.

## Failure Conditions

- Invalid or unsupported recurrence/currency/amount is rejected without saving.
- A cross-household association or unauthorized target is rejected.
- Retrying an identical operation does not duplicate commitment or history.

## Postconditions

The active commitment and its expected future due dates are visible only to authorized members; prior terms and outcomes remain unchanged.
