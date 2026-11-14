---
id: CHG-TASK-INBOX-LIFECYCLES
type: product-change
title: Make Task and Inbox proposal lifecycles explicit
status: applied
base-revision: 2209fe41c377511ca76eb53ac2af17f2f5541094
operations:
  add:
    - LC-TASK
    - LC-INBOX-PROPOSAL
    - SB-INBOX-APPROVE-PROPOSAL
    - SB-INBOX-APPLY-PROPOSAL
    - SB-INBOX-REJECT-PROPOSAL
    - SB-INBOX-UNAUTHORIZED-PROPOSAL
    - SB-INBOX-INVALID-PROPOSAL
  modify:
    - SB-TASKS-COMPLETE-TASK
    - SB-TASKS-CANCEL-TASK
    - FR-INBOX-REVIEW
    - QR-INBOX-PRIVATE-HOUSEHOLD-DATA
  remove:
    - SB-INBOX-REVIEW-PROPOSAL
unaffected:
  - id: UC-INBOX-CAPTURE-AND-REVIEW
    cause: QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    relationship: applies-to
    digest: sha256:3822e409a2d55ec21457dd7e181b9c0017d22ad1c5aeda1e212efcd5652f93d5
    cause-digest: sha256:13175fea8397c7f3c4fba319e63231aac0150c12d04ea532fb0dcfc706696e3f
    reason: The security obligations are unchanged. Verification now names the dedicated unauthorized-approval example. The use case already requires household access and keeps the same failure conditions.
  - id: FR-TASKS-CANCEL-TASK
    cause: SB-TASKS-CANCEL-TASK
    relationship: verification[].scenario-ref
    digest: sha256:5e9f0c5cf475c71536ee549c99d2c638354aa58c6f462e44350123df286b41ae
    cause-digest: sha256:b303095cc87548586f3adf3ecf9a84d66899780ecd2a219b639e7ad65ad02a38
    reason: The example retains its pending-to-cancelled outcome; it adds native lifecycle coverage and clarifies its existing boundary. The cancellation requirement remains satisfied.
  - id: FR-TASKS-COMPLETE-TASK
    cause: SB-TASKS-COMPLETE-TASK
    relationship: verification[].scenario-ref
    digest: sha256:37265014449d4bc6e107f6c0a712fef40f79ca2b9ad05b0f1744d5efea99a9e7
    cause-digest: sha256:3ae707f9f316ff0ea29829ec4d8d1ca8eaddb830fe9e6741c1e8db599ac06dfa
    reason: The example retains its pending-to-completed outcome; it adds native lifecycle coverage and clarifies its existing boundary. The completion requirement remains satisfied.
---

## Problem

Task and Inbox proposal state changes are spread across terms, rules and examples. One Inbox example combines approval and rejection as if their outcomes occurred together.

## Intended Product Outcome

Native lifecycles make the accepted states and transitions explicit. Each Inbox example describes one stimulus and its observable result. Successful examples cover a lifecycle transition; refused actions keep their own examples without claiming a transition.

## Rationale

Juan approved implementation of the product model improvement plan. The change preserves existing obligations and uses accepted product artifacts and current domain events as evidence. Executing is recovered from the existing Inbox model; it is distinct from Applied and does not add a UI requirement.

## Affected Product Areas

Tasks and Inbox proposal review and execution.

## Open Questions

What should a member see and be able to do after an approved action fails during execution? The existing model does not define retry or reset behavior. No such transition is introduced here.

## Product Acceptance

Task completion and cancellation have distinct terminal states. Inbox approval, successful application and rejection have separate examples. Authorization and invalid-action examples do not cover a successful transition. Existing requirement references lead to the replacement examples.

## Out of Scope

Application behavior, retry policies, new UI requirements and household capabilities.
