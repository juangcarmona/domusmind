---
id: TERM-PAYMENT-RECORD
type: domain-term
title: Payment Record
status: draft
defined-in: BC-HOUSEHOLD-FINANCES
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
  - TERM-EXPENSE
---
## Definition

A household-entered fact that a due amount was settled, including the payment date, amount and optional reference, associated with a Financial Due Occurrence. A cycle may have a payment outcome without external account verification.

## Distinguish From

A Financial Commitment is the expectation; a Payment Record is an actual settlement statement. An Expense is the cost fact. Whether a simple payment record also creates or is represented by one Expense is a product-level non-duplication decision; no account transaction or reconciliation is implied.

## Usage

The household can record payment state manually. Partial payment, multiple installments, missed-payment and correction behavior require explicit policy and are not assumed for MVP. Preserve the original expected due amount separately from actual amounts.
