---
id: SB-ASSET-OCCURRENCE-RESCHEDULED
type: structured-behaviour
title: Rescheduling moves the same Pending cycle
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An Active obligation has a Pending cycle due on 31 January and a monthly last-day rule
when: The member reschedules that cycle to 5 February
then:
- The same cycle remains Pending with a retained record of the date change
- Its pending date becomes 5 February
- The original monthly last-day recurrence rule is unchanged
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OCCURRENCE
  transition: RESCHEDULE
---

## Intent

Rescheduling moves the same Pending cycle.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
