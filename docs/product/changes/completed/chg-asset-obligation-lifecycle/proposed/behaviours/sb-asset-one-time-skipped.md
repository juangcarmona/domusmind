---
id: SB-ASSET-ONE-TIME-SKIPPED
type: structured-behaviour
title: A skipped one-time requirement is finished
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The Active obligation is one-time with one Pending cycle
when: The member skips the cycle
then:
- The cycle becomes Skipped
- The obligation becomes terminally Finished
- History remains readable and no new Pending cycle or actionable due projection is created
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OBLIGATION
  transition: FINISH-SKIPPED
---

## Intent

A skipped one-time requirement is finished.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
