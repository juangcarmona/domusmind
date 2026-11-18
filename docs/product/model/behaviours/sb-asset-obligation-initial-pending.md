---
id: SB-ASSET-OBLIGATION-INITIAL-PENDING
type: structured-behaviour
title: A new obligation starts Active with one Pending cycle
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- A supported due rule and local date have been supplied
when: The member defines the obligation
then:
- The obligation is Active with exactly one Pending cycle
- No unbounded future cycle sequence or independent work is created
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

A new obligation starts Active with one Pending cycle.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
