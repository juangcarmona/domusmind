---
id: FR-ASSETS-AGENDA-PROJECTION
type: functional-requirement
title: Project asset due information into Agenda
status: active
derived-from:
- UC-ASSETS-MANAGE-OBLIGATION
- UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
- BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
- BR-ASSET-OBLIGATION-LIFECYCLE
verification:
- scenario: Agenda navigation opens the matching asset obligation and presents any linked Task or Plan as separately
    owned work without silent status synchronization.
---

## Requirement

Agenda can display upcoming and overdue Asset Obligation due information with its asset identity and navigation to the obligation. Projection updates after obligation lifecycle changes; any explicitly linked Task or Calendar Plan appears as a separate, distinguishable item with a visible link to the obligation, remains independently owned and is not silently synchronized.

Only an Active obligation on an Active Asset contributes actionable upcoming or overdue cycle information. Suspension, cancellation, one-time completion/skip and Asset archive remove that actionable projection while preserving consultation and history. Reactivation or Asset restoration contributes the retained pending date only when both lifecycles allow it.

## Rationale

Agenda is DomusMind's unified temporal read surface; users should see what must happen next without reclassifying an obligation as execution.
