---
id: SB-ASSET-OBLIGATION-CANCELLED-REFUSES-SKIP
type: structured-behaviour
title: Cancelled obligations refuse a new skip action
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Cancelled
- The request is a new operation, not an exact retry of an accepted one
when: The member attempts to skip a cycle
then:
- The new action is refused
- Obligation state, pending information and recorded history remain unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Cancelled obligations refuse a new skip action.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
