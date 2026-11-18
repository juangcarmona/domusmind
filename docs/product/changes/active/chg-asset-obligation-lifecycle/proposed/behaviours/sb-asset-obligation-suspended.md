---
id: SB-ASSET-OBLIGATION-SUSPENDED
type: structured-behaviour
title: Suspension retains the pending cycle
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Active with a Pending cycle due on 31 January
when: The member suspends the obligation
then:
- The obligation becomes Suspended
- The same Pending cycle and date remain stored with unchanged history
- The obligation contributes no actionable due projection
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OBLIGATION
  transition: SUSPEND
---

## Intent

Suspension retains the pending cycle.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
