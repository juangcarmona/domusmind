---
id: QR-ASSETS-RECURRENCE-AND-HISTORY
type: quality-requirement
title: Keep recurrence progression and recorded history correct
status: active
quality-attribute: "reliability"
applies-to:
  - BC-HOUSEHOLD-ASSETS
  - UC-ASSETS-MANAGE-OBLIGATION
  - UC-ASSETS-COMPLETE-OBLIGATION
verification:
  - scenario: "Deterministic tests cover a 31 January monthly last-day anchor through February and March, one overdue pending cycle across missed months, completion-relative progression from actual completion or a skipped due date, manager-configured household-local dates across device timezones and daylight-saving transitions, explicit skip/reschedule, retry/idempotency, concurrent completion and historical retention."
---
## Requirement

Due-state calculation and occurrence transitions must be deterministic for household-local dates, timezone-aware and idempotent. The first release does not require exact due times. Concurrent or retried completion cannot create duplicate history or silently lose a recorded outcome.

## Measurement

Automated tests cover agreed date semantics, duplicate requests and concurrent updates; integration tests verify Agenda projection consistency after transitions.
