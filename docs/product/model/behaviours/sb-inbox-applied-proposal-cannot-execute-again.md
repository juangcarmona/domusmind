---
id: SB-INBOX-APPLIED-PROPOSAL-CANNOT-EXECUTE-AGAIN
type: structured-behaviour
title: An Applied proposal cannot execute twice
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - A proposal is Applied and linked to its successful result.
when: A member attempts to execute that proposal again.
then:
  - The owning-area action is not invoked again.
  - The proposal remains Applied and linked to its existing result.
uses-terms:
  - TERM-INBOX-PROPOSAL
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Keep a repeated approval attempt from duplicating the household action.

## Boundaries

The example defines the product outcome without prescribing an idempotency mechanism.
