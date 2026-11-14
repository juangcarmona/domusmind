---
id: SB-INBOX-RECOVER-UNAPPLIED-FAILURE
type: structured-behaviour
title: A failed unapplied proposal returns for correction
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - A proposal is Executing after explicit approval.
  - Its owning-area action fails without applying the household change.
when: The failed attempt is reported.
then:
  - The proposal returns to Pending with its details retained.
  - The member sees the failure reason.
  - The action is not retried automatically.
covers-transition:
  lifecycle: LC-INBOX-PROPOSAL
  transition: RECOVER-UNAPPLIED-FAILURE
uses-terms:
  - TERM-INBOX-PROPOSAL
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Give the member a clear, editable recovery point after an unapplied action fails.

## Boundaries

An action already recorded as Applied is not reset or executed again.
