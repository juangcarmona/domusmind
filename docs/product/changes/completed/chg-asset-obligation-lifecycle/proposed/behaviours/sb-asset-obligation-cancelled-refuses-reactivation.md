---
id: SB-ASSET-OBLIGATION-CANCELLED-REFUSES-REACTIVATION
type: structured-behaviour
title: A cancelled requirement cannot be reopened
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Cancelled
when: The member attempts to reactivate the obligation
then:
- The action is refused
- The Cancelled obligation and its history remain unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

A cancelled requirement cannot be reopened.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
