---
id: SB-ASSET-RESTORED-ASSET-KEEPS-OBLIGATION-SUSPENDED
type: structured-behaviour
title: Restoring an Asset does not reactivate a Suspended obligation
status: active
illustrates:
- UC-ASSETS-MANAGE-OBLIGATION
- BR-ASSET-OBLIGATION-LIFECYCLE
given:
- A member is authorized to restore an Archived Asset
- Its obligation was Suspended before archive
when: The member explicitly restores the Asset
then:
- The obligation remains Suspended with its unchanged pending cycle
- The obligation contributes no actionable due projection until explicitly reactivated
uses-terms:
- TERM-ASSET-OBLIGATION
- TERM-ASSET-OCCURRENCE
---

## Intent

Restoring an Asset does not reactivate a Suspended obligation.

## Boundaries

Independent work, traceable correction effects and other calendar anchors remain governed separately.
