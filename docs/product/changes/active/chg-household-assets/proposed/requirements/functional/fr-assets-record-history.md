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
  - scenario: "Completing an occurrence twice with the same operation leaves one linked visible history entry; a mistaken completion gains a traceable correction without losing the original; sale or archive preserves read-only history."
---
## Requirement

An authorized member can complete, skip or reschedule a due occurrence and can record unscheduled maintenance, repair, inspection, installation or other supported history. The system retains bounded occurrence history and linked Asset Records as one visible entry per outcome without duplicate outcomes on retry. A mistaken completion is corrected by adding a traceable correction that preserves the original outcome. After sale or archive, history remains readable but not ordinarily editable. Optional actual costs carry an amount and currency on each historical record, not an accounting ledger.

## Rationale

Households need to know what was done and what it cost, including work that was not scheduled in DomusMind.
