---
id: CHG-ASSET-OBLIGATION-LIFECYCLE
type: product-change
title: Define asset obligation and occurrence lifecycle actions
status: approved
base-revision: f857ed15ed50bb5bf8f6e08356c7d0d65c6d5094
operations:
  add:
  - BR-ASSET-OBLIGATION-LIFECYCLE
  - LC-ASSET-OBLIGATION
  - LC-ASSET-OCCURRENCE
  - SB-ASSET-OBLIGATION-INITIAL-PENDING
  - SB-ASSET-OBLIGATION-SUSPENDED
  - SB-ASSET-OBLIGATION-REACTIVATED-OVERDUE
  - SB-ASSET-OBLIGATION-CANCELLED-FROM-ACTIVE
  - SB-ASSET-OBLIGATION-CANCELLED-FROM-SUSPENDED
  - SB-ASSET-OCCURRENCE-CANCELLED
  - SB-ASSET-ONE-TIME-COMPLETED
  - SB-ASSET-ONE-TIME-SKIPPED
  - SB-ASSET-OCCURRENCE-COMPLETED
  - SB-ASSET-OCCURRENCE-SKIPPED
  - SB-ASSET-OCCURRENCE-RESCHEDULED
  - SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-COMPLETE
  - SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-SKIP
  - SB-ASSET-OBLIGATION-SUSPENDED-REFUSES-RESCHEDULE
  - SB-ASSET-OBLIGATION-CANCELLED-REFUSES-COMPLETE
  - SB-ASSET-OBLIGATION-CANCELLED-REFUSES-SKIP
  - SB-ASSET-OBLIGATION-CANCELLED-REFUSES-RESCHEDULE
  - SB-ASSET-OBLIGATION-FINISHED-REFUSES-COMPLETE
  - SB-ASSET-OBLIGATION-FINISHED-REFUSES-SKIP
  - SB-ASSET-OBLIGATION-FINISHED-REFUSES-RESCHEDULE
  - SB-ASSET-OBLIGATION-CANCELLED-REFUSES-REACTIVATION
  - SB-ASSET-OCCURRENCE-RETRY-DOES-NOT-ADVANCE-NEXT
  - SB-ASSET-OCCURRENCE-CONCURRENT-OUTCOMES
  - SB-ASSET-ARCHIVED-ASSET-REFUSES-OBLIGATION-COMPLETION
  - SB-ASSET-RESTORED-ASSET-RETAINS-OBLIGATION-DATE
  - SB-ASSET-RESTORED-ASSET-KEEPS-OBLIGATION-SUSPENDED
  modify:
  - UC-ASSETS-MANAGE-OBLIGATION
  - UC-ASSETS-COMPLETE-OBLIGATION
  - TERM-ASSET-OBLIGATION
  - TERM-ASSET-OCCURRENCE
  - FR-ASSETS-MANAGE-OBLIGATIONS
  - FR-ASSETS-AGENDA-PROJECTION
  - QR-ASSETS-RECURRENCE-AND-HISTORY
  remove: []
unaffected:
- id: BC-HOUSEHOLD-ASSETS
  cause: QR-ASSETS-RECURRENCE-AND-HISTORY
  relationship: applies-to
  digest: sha256:c906b13f8ea4ee4771790626dc32e494b4d4f705885fba1e4939e04359c498b0
  cause-digest: sha256:be4e0698bb040450abe3b0707b6524f0b81034b8f8cc88303c576523d25d9245
  reason: Obligation and occurrence lifecycle remain in the same existing bounded context; no ownership boundary
    changes.
- id: FR-ASSETS-RECORD-HISTORY
  cause: UC-ASSETS-COMPLETE-OBLIGATION
  relationship: derived-from
  digest: sha256:9e4e6473c235b06d78248e678734419f9f8202b0e9a35d8c4040a0f2307fd74f
  cause-digest: sha256:efc09ebd3775a7eae4f1ffa30ff508417f0c9dd69d033567de5daa411102fd7e
  reason: The existing single visible outcome, durable history, optional actual costs and separate traceable correction
    remain unchanged.
- id: FR-ASSETS-REFERENCE-DOCUMENTS
  cause: UC-ASSETS-COMPLETE-OBLIGATION
  relationship: derived-from
  digest: sha256:bee3f5f7f6e8648f1b8c0af6290a76a57668e720b9ccdb346332cbfcb17bb877
  cause-digest: sha256:efc09ebd3775a7eae4f1ffa30ff508417f0c9dd69d033567de5daa411102fd7e
  reason: Existing authorized metadata, references and independent source access remain unchanged; no document grant
    or storage lifecycle is added.
- id: JRN-ASSETS-KEEP-HOUSEHOLD-READY
  cause: UC-ASSETS-COMPLETE-OBLIGATION
  relationship: steps[].use-case
  digest: sha256:6fd8373e879872566c89e4f9293259db837f24d9ea291e123eb15cff6472436c
  cause-digest: sha256:efc09ebd3775a7eae4f1ffa30ff508417f0c9dd69d033567de5daa411102fd7e
  reason: The existing define, fulfill and review journey remains valid; the proposal specifies lifecycle guards
    rather than a new journey.
- id: JRN-ASSETS-KEEP-HOUSEHOLD-READY
  cause: UC-ASSETS-MANAGE-OBLIGATION
  relationship: steps[].use-case
  digest: sha256:6fd8373e879872566c89e4f9293259db837f24d9ea291e123eb15cff6472436c
  cause-digest: sha256:2ed6f1769d2cc2db3ea04b988d6ccf0ccbc257c770a7ec5b50ee83fcc9976fc6
  reason: The existing define, fulfill and review journey remains valid; the proposal specifies lifecycle guards
    rather than a new journey.
- id: BR-ASSET-RECURRENCE-DUE-STATE
  cause: TERM-ASSET-OBLIGATION
  relationship: uses-terms
  digest: sha256:d57ed70f4117a4372e6f7915d6be847be900821d07eb33c0cedbf0d921a3cf8a
  cause-digest: sha256:b12810fe9f450d846ace65c0fcb0d20ca988dd3fa9b2a4b9c2cbbb162d9d986d
  reason: Existing household-local dates, one pending overdue cycle, monthly last-day and completion-relative progression
    are preserved without selecting new calendar or timezone-change rules.
- id: CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES
  cause: TERM-ASSET-OBLIGATION
  relationship: uses-terms
  digest: sha256:cdc3766e60717f042d2a1c5b99f93b4e62ec710bfaa9fa7c4b7fe798d661a659
  cause-digest: sha256:b12810fe9f450d846ace65c0fcb0d20ca988dd3fa9b2a4b9c2cbbb162d9d986d
  reason: Obligations keep their existing context ownership and household authorization; no work is created or synchronized
    automatically.
- id: BR-ASSET-RECURRENCE-DUE-STATE
  cause: TERM-ASSET-OCCURRENCE
  relationship: uses-terms
  digest: sha256:d57ed70f4117a4372e6f7915d6be847be900821d07eb33c0cedbf0d921a3cf8a
  cause-digest: sha256:5caa931f6fbe716e38ca90545008ede8ec24181a10561eedc4bf50489ec160f6
  reason: Existing household-local dates, one pending overdue cycle, monthly last-day and completion-relative progression
    are preserved without selecting new calendar or timezone-change rules.
---

## Problem

Accepted Asset Obligation prose describes suspension, cancellation and occurrence outcomes, but does not completely define their initial and terminal states, reactivation, forbidden actions or the guards between Asset and obligation state. The household cannot rely on one complete rule for when a due cycle is actionable and when retained information is read-only.

## Intended Product Outcome

The accepted future state distinguishes Active, Suspended, Cancelled and Finished obligations and Pending, Completed, Skipped and Cancelled cycles. Temporary suspension preserves the pending date; explicit reactivation retains any overdue state. Cancellation is terminal and distinct from an explicit skip. A resolved one-time requirement is Finished. Recurring outcomes retain the accepted recurrence progression and create only one next Pending cycle. Refused actions, exact retries and competing outcomes preserve one durable result and cannot act on a later cycle accidentally.

Asset archive blocks obligation mutation while retaining states and dates. Restoration does not silently reactivate a separately Suspended obligation. Only Active obligations on Active Assets contribute actionable due information; consultation and existing history remain available under authorization.


## Rationale

A temporary pause, cancellation and a completed one-time requirement have different household meanings. Explicit lifecycle rules prevent forgotten overdue cycles, invented skipped work and changed historical outcomes. Existing monthly last-day, completion-relative and bounded-history rules remain authoritative and unchanged.

On 18 November 2026 the product owner approved the complete proposal and its application, answering “Si a las 4” to the request to apply the four recommended rules and resume child #358. The decisions authorize reactivation with the retained pending date, terminal cancellation without an invented completion or skip, terminal resolution of a one-time requirement, and the Archived Asset obligation guard. The proposed future-state artifacts reviewed in commit `6af016450f0554b799c174a7a6b38c05bbc779f6` are unchanged.

## Affected Product Areas

Asset obligation and occurrence states, lifecycle actions, actionable due projection eligibility and preservation of household history. Existing household authorization and independent work ownership remain unchanged. The eight source-bound unaffected entries in this change account for direct graph impact. The Assets context, existing history and document requirements, household journey, recurrence rule and ownership constraint retain their existing meaning; no new correction, document or independent-work lifecycle is selected.

## Open Questions

None.

## Product Acceptance

The owner has selected the temporary-suspension, terminal-cancellation, one-time-resolution and Asset-archive rules. The two lifecycles declare their source states, transitions and terminal states. Structured behaviours express permitted actions, refusals, exact retries, conflicting outcomes and retention of pending dates and history.

## Out of Scope

Other calendar anchors and household timezone-change behavior; usage-meter recurrence; automatic dated reactivation; sale-specific workflows and definitive deletion; new correction semantics or their effect on future due state; Asset Record cost fields; document grants and binary storage; UI naming and presentation; technical contracts, storage, event names and transport status codes; implementation, execution evidence, integration, release and deployment. Tasks, Plans, Routines and Lists retain independent ownership with no automatic creation or silent synchronization.
