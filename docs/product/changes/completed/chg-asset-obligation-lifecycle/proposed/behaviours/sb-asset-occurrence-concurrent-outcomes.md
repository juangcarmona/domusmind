---
id: SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES
type: structured-behaviour
title: Competing outcomes cannot both resolve a cycle
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An Active obligation has one Pending cycle
when: The member submits competing completion and skip operations concurrently
then:
- Exactly one outcome is accepted for that cycle
- The conflicting operation cannot overwrite the outcome or resolve a later cycle
- The next due state is calculated once from the accepted outcome
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Competing outcomes cannot both resolve a cycle.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
