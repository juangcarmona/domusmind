---
id: SB-ASSET-OCCURRENCE-RETRY-DOES-NOT-ADVANCE-NEXT
type: structured-behaviour
title: An exact retry never resolves the next cycle
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An accepted completion operation has resolved one cycle and established a next Pending cycle
when: The member retries that exact accepted completion operation
then:
- The earlier accepted outcome is returned
- History is not duplicated and the next Pending cycle is unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

An exact retry never resolves the next cycle.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
