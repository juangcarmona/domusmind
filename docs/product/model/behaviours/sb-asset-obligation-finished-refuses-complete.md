---
id: SB-ASSET-OBLIGATION-FINISHED-REFUSES-COMPLETE
type: structured-behaviour
title: Finished obligations refuse a new complete action
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Finished
- The request is a new operation, not an exact retry of an accepted one
when: The member attempts to complete a cycle
then:
- The new action is refused
- Obligation state, pending information and recorded history remain unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Finished obligations refuse a new complete action.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
