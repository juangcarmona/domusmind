---
id: SB-ASSET-ARCHIVED-ASSET-REFUSES-OBLIGATION-COMPLETION
type: structured-behaviour
title: Archived Assets refuse new obligation outcomes
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- A member is authorized for an Archived Asset
- An associated obligation is Active with a Pending cycle
when: The member attempts a new completion of the cycle
then:
- The completion is refused
- The obligation, pending date and recorded history remain unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Archived Assets refuse new obligation outcomes.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
