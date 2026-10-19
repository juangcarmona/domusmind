---
id: FR-ASSETS-RECORD-HISTORY
type: functional-requirement
title: Preserve asset outcomes, records and actual costs
status: draft
derived-from:
  - UC-ASSETS-COMPLETE-OBLIGATION
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - BR-ASSET-OCCURRENCE-HISTORY
verification:
  - scenario: "Completing an occurrence twice with the same operation does not duplicate its history record; unscheduled repairs can be recorded independently."
---
## Requirement

An authorized member can complete, skip or reschedule a due occurrence and can record unscheduled maintenance, repair, inspection, installation or other supported history. The system retains bounded occurrence history and linked Asset Records without duplicate outcomes on retry; optional actual costs are associated with the historical record, not treated as an accounting ledger.

## Rationale

Households need to know what was done and what it cost, including work that was not scheduled in DomusMind.
