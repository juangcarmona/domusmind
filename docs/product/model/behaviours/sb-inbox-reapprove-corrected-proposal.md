---
id: SB-INBOX-REAPPROVE-CORRECTED-PROPOSAL
type: structured-behaviour
title: A member approves a corrected failed proposal
status: active
illustrates:
  - UC-INBOX-CAPTURE-AND-REVIEW
  - BR-INBOX-REVIEW-AND-EXECUTION
given:
  - An unapplied failed proposal is Pending.
  - An authorized member has corrected the proposal details.
when: The member explicitly approves the corrected proposal.
then:
  - The proposal enters execution through its owning area.
  - The owning area checks its current authorization and validation rules.
covers-transition:
  lifecycle: LC-INBOX-PROPOSAL
  transition: APPROVE
uses-terms:
  - TERM-INBOX-PROPOSAL
provenance:
  source: 'product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: interview
---

## Intent

Let the member start a new approved attempt after correcting the failure.

## Boundaries

Editing alone does not execute an action or authorize a retry.
