---
id: UC-FINANCES-RECORD-COST
type: use-case
title: Record an actual household expense or commitment payment
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-FINANCES
governed-by:
  - BR-FINANCES-EXPENSE-PAYMENT-SEPARATION
uses-terms:
  - TERM-EXPENSE
  - TERM-PAYMENT-RECORD
  - TERM-FINANCIAL-DUE-OCCURRENCE
---
## Goal

Record a cost that happened and, when applicable, its payment against a commitment due cycle.

## Trigger

A member pays or receives a bill, incurs an unscheduled cost such as a repair, or wants to update the settlement state of a known due cycle.

## Preconditions

- The member is authenticated and authorized to create or inspect the relevant household financial record.
- Amount and currency are explicit; associations target the same household and are permitted.

## Main Flow

1. The member captures the actual amount, currency, relevant date, description/category and optional contextual attribution.
2. The member optionally links the cost to a Financial Commitment and due occurrence and records payment date/reference where needed.
3. Finances applies the approved expense/payment non-duplication rule and validates the occurrence, currency and access policy.
4. Finances stores one durable actual cost fact and the associated payment outcome, preserving the expected amount separately.
5. Finances updates authorized history, spending summaries and the due-state projection.

## Alternative Flows

- An unscheduled expense is recorded without a commitment.
- A variable bill's actual amount differs from its expected amount; both remain distinguishable.
- Multiple partial payments may be recorded only if product owners enable that policy for the delivered scope.
- A correction is made using the approved audit-preserving correction flow.

## Failure Conditions

- Missing currency/date, unsupported precision, invalid amount or unauthorized target rejects the operation.
- A duplicate retry does not add the same expense/payment twice.
- A settlement cannot be applied to a cancelled, unknown or cross-household occurrence.

## Postconditions

The actual cost and payment outcome are recorded without rewriting commitment terms or double-counting the same economic event.
