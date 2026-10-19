---
id: FR-ASSETS-AGENDA-PROJECTION
type: functional-requirement
title: Project asset due information into Agenda
status: draft
derived-from:
  - UC-ASSETS-MANAGE-OBLIGATION
  - UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
  - BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
verification:
  - scenario: "Agenda navigation opens the matching asset obligation and presents any linked Task or Plan as separately owned work without silent status synchronization."
---
## Requirement

Agenda can display upcoming and overdue Asset Obligation due information with its asset identity and navigation to the obligation. Projection updates after obligation lifecycle changes; any separately linked Task or Calendar Plan remains distinguishable, independently owned and not silently synchronized.

## Rationale

Agenda is DomusMind's unified temporal read surface; users should see what must happen next without reclassifying an obligation as execution.
