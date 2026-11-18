---
id: SB-ASSET-RESTORED-ASSET-RETAINS-OBLIGATION-DATE
type: structured-behaviour
title: Restoring an Asset preserves an Active obligation pending date
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- A member is authorized to restore an Archived Asset
- Its Active obligation has a Pending cycle due on 31 January
- The current household-local date is 15 March
when: The member explicitly restores the Asset
then:
- The obligation remains Active with the same overdue 31 January cycle
- No obligation outcomes or missed cycles are invented
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Restoring an Asset preserves an Active obligation pending date.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
