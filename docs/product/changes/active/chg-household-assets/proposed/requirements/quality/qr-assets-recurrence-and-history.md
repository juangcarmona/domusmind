---
id: QR-ASSETS-RECURRENCE-AND-HISTORY
type: quality-requirement
title: Keep recurrence progression and recorded history correct
status: draft
quality-attribute: "reliability"
applies-to:
  - BC-HOUSEHOLD-ASSETS
  - UC-ASSETS-MANAGE-OBLIGATION
  - UC-ASSETS-COMPLETE-OBLIGATION
verification:
  - scenario: "Deterministic tests cover timezone and calendar boundaries, month-end and daylight-saving transitions, retry/idempotency, concurrent completion, skip/reschedule, missed due dates and historical retention."
---
## Requirement

Due-state calculation and occurrence transitions must be deterministic, timezone-aware and idempotent. Concurrent or retried completion cannot create duplicate history or silently lose a recorded outcome.

## Measurement

Automated tests cover agreed date semantics, duplicate requests and concurrent updates; integration tests verify Agenda projection consistency after transitions.
