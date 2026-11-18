---
id: SB-ASSET-OCCURRENCE-SKIPPED
type: structured-behaviour
title: An explicit skip records one terminal cycle outcome
status: active
illustrates:
- UC-ASSETS-COMPLETE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- An authorized household member is acting on an Active Asset
- An Active completion-relative obligation has a Pending cycle due on 31 January
- The rule advances by twelve months
when: The member skips that cycle on 15 March
then:
- That cycle becomes terminally Skipped
- The obligation remains Active with exactly one next Pending cycle calculated from 31 January, not 15 March
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
covers-transition:
  lifecycle: LC-ASSET-OCCURRENCE
  transition: SKIP
---

## Intent

An explicit skip records one terminal cycle outcome.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
