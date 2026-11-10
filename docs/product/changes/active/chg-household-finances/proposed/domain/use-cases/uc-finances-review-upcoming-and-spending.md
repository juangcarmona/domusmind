---
id: UC-FINANCES-REVIEW-UPCOMING-AND-SPENDING
type: use-case
title: Review upcoming commitments and household spending
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-FINANCES
governed-by:
  - BR-FINANCES-COMMITMENT-HISTORY
  - BR-FINANCES-AGENDA-PROJECTION
  - BR-FINANCES-CURRENCY-SAFE-SUMMARIES
uses-terms:
  - TERM-FINANCIAL-COMMITMENT
  - TERM-FINANCIAL-DUE-OCCURRENCE
  - TERM-EXPENSE
---
## Goal

Understand what is expected soon and what the household actually spent over a selected period.

## Trigger

A member opens Finances, asks what payments are coming, or reviews historical costs by time, category or authorized household context.

## Preconditions

- The member has access to the household and the financial records requested.
- The requested period and currency context are unambiguous.

## Main Flow

1. Finances presents upcoming due occurrences separately from actual expenses and payment outcomes.
2. The member selects a period, category, Asset, Member, Area or provider filter.
3. Finances returns authorized expense history and expected-versus-actual values with currency-aware totals.
4. The member opens an item in its owning Finances or linked context surface.

## Alternative Flows

- An expense has no linked commitment and remains in actual-cost history.
- Several currencies are present; totals are grouped by currency, not converted.
- An item is linked to an Asset or Document only where that context contract and access permit.
- An authorized Agenda view opens a Finance due entry in Finances; Agenda remains read-only.

## Failure Conditions

- Unauthorized financial records are omitted without leaking labels, amount, count or existence.
- An invalid period or unsupported currency filter is rejected or explained without partial totals.

## Postconditions

The member sees a truthful, access-aware distinction between expected commitments, actual costs and recorded settlements.
