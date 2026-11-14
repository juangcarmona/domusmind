---
id: BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
type: business-rule
title: Asset obligations remain distinct from execution records
status: active
---
## Rule

An Asset Obligation defines a persistent requirement and due rule. It does not create a Task, Routine, Calendar Plan or List item automatically. A member may explicitly create or link such work; its owning context retains validation, authorization and lifecycle. Agenda displays a linked obligation and Task or Calendar Plan as two distinguishable items with an explicit relationship, without implying duplicate ownership or silent status synchronization.

## Rationale

This preserves DomusMind's separation of time, execution, responsibility and reusable household memory.

## Examples

- A boiler's annual service appears as due in Agenda without requiring a Task.
- A member may separately create a Task to call a technician; completing it does not silently record the service as complete.
- A service appointment is a Calendar Plan linked to, but not substituted for, the obligation.

## Exceptions

Any future automation requires a separate product decision and explicit household intent.
