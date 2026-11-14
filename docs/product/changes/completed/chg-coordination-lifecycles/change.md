---
id: CHG-COORDINATION-LIFECYCLES
type: product-change
title: Connect household coordination lifecycles to accepted examples
status: applied
base-revision: cb0cfa247b66b494652f5a8766d21b223bba87a7
operations:
  add:
    - LC-ROUTINE
    - SB-TASKS-DELETE-ROUTINE
    - LC-LIST
    - LC-PLAN
    - LC-EXTERNAL-CALENDAR-CONNECTION
    - SB-CALENDAR-START-SYNC
    - SB-CALENDAR-SYNC-SUCCESS
    - SB-CALENDAR-SYNC-PARTIAL-FAILURE
    - SB-CALENDAR-SYNC-FAILURE
    - SB-CALENDAR-AUTH-EXPIRED
  modify:
    - SB-TASKS-PAUSE-ROUTINE
    - SB-TASKS-RESUME-ROUTINE
    - SB-TASKS-PAUSE-UNTIL-AUTO-RESUME
    - FR-TASKS-DELETE-ROUTINE
    - UC-TASKS-DELETE-ROUTINE
    - SB-LISTS-ARCHIVE
    - SB-LISTS-RESTORE
    - SB-LISTS-DELETE
    - SB-CALENDAR-CANCEL-PLAN
    - SB-CALENDAR-DISCONNECT
    - FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    - FR-CALENDAR-SHOW-CONNECTION-STATUS
  remove: []
unaffected:
  - id: FR-CALENDAR-CANCEL-PLAN
    cause: SB-CALENDAR-CANCEL-PLAN
    relationship: verification[].scenario-ref
    digest: sha256:53b73f69861626f442cc19410a78860736db1ec7aafcb119b1e7e7d3520cc598
    cause-digest: sha256:0f3fcb8c9fb6afd6778ae7e62177eb35fe720a75e558228cfc880f6fd76a2bf3
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-CANCEL-PLAN makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
    cause: SB-CALENDAR-DISCONNECT
    relationship: verification[].scenario-ref
    digest: sha256:7bfdd307fcfa7a2c2144d03ccd965f0128ce4b48a8f121cba03ef139b7de5f3b
    cause-digest: sha256:08ccad7fc87478aee53ebac67db39444dd69f4badb02aff22d6b89e14f8b8e06
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-DISCONNECT makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-LISTS-ARCHIVE-LIST
    cause: SB-LISTS-ARCHIVE
    relationship: verification[].scenario-ref
    digest: sha256:050b27be17e6b043e91241e56988e8099bc62e18ff07fe964842e197bfe11477
    cause-digest: sha256:6c0a6878c819ada87235cd49ddfa67d8490441f05dc2a563a3f484f02bc04bf0
    reason: The linked artifact retains its existing product obligations. SB-LISTS-ARCHIVE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-LISTS-DELETE-LIST
    cause: SB-LISTS-DELETE
    relationship: verification[].scenario-ref
    digest: sha256:b69bc2dc16ba6287b5c1de7d2f1344f2c863ad0e30c7ce5bd4a80b4f3ed890f2
    cause-digest: sha256:15324945d2c611f22a2e399e76175a9cb63f89315d8e3e54eb5b680cee358e2b
    reason: The linked artifact retains its existing product obligations. SB-LISTS-DELETE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-LISTS-RESTORE-LIST
    cause: SB-LISTS-RESTORE
    relationship: verification[].scenario-ref
    digest: sha256:0deba81893a356379ca3ba12744d9856c42b1a53fc7371cae53853b665bb00b3
    cause-digest: sha256:aeecdcaa3868472dad35760476ef751faf801c90db5841359f38e26ef4b57e0b
    reason: The linked artifact retains its existing product obligations. SB-LISTS-RESTORE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-TASKS-PAUSE-ROUTINE
    cause: SB-TASKS-PAUSE-ROUTINE
    relationship: verification[].scenario-ref
    digest: sha256:8059ca8e3c8a08d6d4627760012c7ee4083b609c6c45182d54c4ab5cb6fdef35
    cause-digest: sha256:0f644c50dcaba4ad549c1d51fceaaf33c6dd9bc46e7077158f62630188838641
    reason: The linked artifact retains its existing product obligations. SB-TASKS-PAUSE-ROUTINE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-TASKS-PAUSE-ROUTINE-UNTIL
    cause: SB-TASKS-PAUSE-UNTIL-AUTO-RESUME
    relationship: verification[].scenario-ref
    digest: sha256:62f7787cf2a6fb3ef3cf1f00f1fd05ea435746ddf84f56bc9405215dcbd9fdc0
    cause-digest: sha256:57868fef1d22d7a2c8126356c88020882a9ba9e06d76bb9462045c2b82405d32
    reason: The linked artifact retains its existing product obligations. SB-TASKS-PAUSE-UNTIL-AUTO-RESUME makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-TASKS-RESUME-ROUTINE
    cause: SB-TASKS-RESUME-ROUTINE
    relationship: verification[].scenario-ref
    digest: sha256:ca14a5d469152463f35cf906cc9e4e84cd6f987682c54d4710f5c30744e29052
    cause-digest: sha256:d599e1ba868b9f3e6ff952ddfab86373427a3f256dfcf2f832a85de2d543903e
    reason: The linked artifact retains its existing product obligations. SB-TASKS-RESUME-ROUTINE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
---

## Problem

Routine, List, Calendar Plan and connection states are scattered across terms, rules and verification prose. Domain events are present in the implementation, but readers cannot follow the state changes from the product graph.

## Intended Product Outcome

Four native lifecycles connect accepted states, governed transitions, use cases and concrete examples. Successful examples name the transition they cover. Deletion is represented as a terminal product outcome even when the record is removed from storage.

## Rationale

Juan approved the product model improvement plan. This batch follows the validated Task and Inbox pilot and keeps the existing acceptance decisions, including dated Routine resumption and the Calendar status catalog. Existing domain events serve as implementation evidence, not as a separate product artifact type.

## Affected Product Areas

Tasks and Routines, Lists, Calendar Plans and external calendar connections.

## Open Questions

The accepted Calendar catalog includes Needs attention and Rehydrating, but current sync orchestration does not define entry triggers for them. Meal Plan completion also retains its existing deferred trigger decision. No trigger is invented for any of these states. AuthExpired can be overwritten by Failed in the current sync error path; this is recorded as an implementation discrepancy.

## Product Acceptance

The graph has native lifecycle nodes and transition coverage for the accepted actions. Negative examples do not claim successful transitions. The model distinguishes accepted intent from delivered implementation and keeps all existing permissions, validation conditions and outcomes.

## Out of Scope

Changes to application behavior, implementation of dated pauses, synchronization error recovery and new product decisions.
