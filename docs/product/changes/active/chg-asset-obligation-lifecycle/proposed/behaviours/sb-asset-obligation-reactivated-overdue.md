---
id: SB-ASSET-OBLIGATION-REACTIVATED-OVERDUE
type: structured-behaviour
title: Reactivation retains the overdue date
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- The obligation is Suspended with a Pending cycle due on 31 January
- The current household-local date is 15 March
when: The member reactivates the obligation
then:
- The same obligation becomes Active
- The same Pending cycle is overdue on its unchanged 31 January date
- No missed cycles or outcomes are invented
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OBLIGATION
  transition: REACTIVATE
---

## Intent

Reactivation retains the overdue date.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
