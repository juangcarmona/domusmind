---
id: TERM-FINANCIAL-DUE-OCCURRENCE
type: domain-term
title: Financial Due Occurrence
status: draft
defined-in: BC-HOUSEHOLD-FINANCES
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-PAYMENT-RECORD
---
## Definition

One dated, expected payment cycle calculated from a Financial Commitment's recurrence and effective amount terms. It may have an expected amount, due date and a recorded payment outcome.

## Distinguish From

A due occurrence is not a Calendar Plan, Task, Agenda-owned record or actual Expense. A projected future cycle need not be stored as a persistent entity; a cycle with an explicit payment outcome may require bounded historical state.

## Usage

Calculate dates using the household's calendar/timezone semantics and a defined month-end rule. Do not materialize an unbounded future. Preserve an expected amount snapshot when a cycle's status is recorded so later price changes do not alter history.
