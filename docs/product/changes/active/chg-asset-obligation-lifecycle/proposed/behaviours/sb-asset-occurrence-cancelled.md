---
id: SB-ASSET-OCCURRENCE-CANCELLED
type: structured-behaviour
title: Cancellation preserves the distinction from a skip
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An Active obligation has a Pending cycle
when: The member cancels the obligation
then:
- The current cycle becomes terminally Cancelled
- No Completed or Skipped outcome is invented
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OCCURRENCE
  transition: CANCEL
---

## Intent

Cancellation preserves the distinction from a skip.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
