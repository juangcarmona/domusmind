---
id: SB-ASSET-OBLIGATION-CANCELLED-FROM-SUSPENDED
type: structured-behaviour
title: Cancellation ends a suspended obligation
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Suspended with a Pending cycle and prior history
when: The member cancels the obligation
then:
- The obligation becomes terminally Cancelled
- Its Pending cycle becomes Cancelled without an implied completion or skip
- Prior history remains readable and no actionable due projection remains
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OBLIGATION
  transition: CANCEL
---

## Intent

Cancellation ends a suspended obligation.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
