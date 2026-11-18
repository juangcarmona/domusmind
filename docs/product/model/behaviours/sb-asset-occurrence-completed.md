---
id: SB-ASSET-OCCURRENCE-COMPLETED
type: structured-behaviour
title: Completion records one terminal cycle outcome
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An Active recurring obligation has a Pending cycle
when: The member records completion with its actual completion date
then:
- That cycle becomes terminally Completed with one visible recorded outcome
- The obligation remains Active with exactly one next Pending cycle under its accepted recurrence rule
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OCCURRENCE
  transition: COMPLETE
---

## Intent

Completion records one terminal cycle outcome.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
