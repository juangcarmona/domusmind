---
id: CHG-MODEL-READABILITY
type: product-change
title: Clarify examples and product language across household flows
status: applied
base-revision: 4ceb6f203b17ebad063895b7f9c982430982aab3
operations:
  add:
    - SB-LISTS-ARCHIVED-ITEM-ADD-REFUSED
    - SB-LISTS-ARCHIVED-ITEM-CHECK-REFUSED
    - SB-MEALS-ASSIGN-RECIPE-DRAFT
    - SB-MEALS-ASSIGN-RECIPE-ACTIVE
    - SB-RECIPES-DELETE-USED-BY-DRAFT
    - SB-RECIPES-DELETE-USED-BY-COMPLETED
    - SB-RECIPES-DELETE-USED-BY-MIXED
    - SB-WEB-REQUEST-DISCONNECT
    - SB-WEB-CONFIRM-DISCONNECT
    - SB-INBOX-CAPTURE-NOTE
    - SB-CHAT-STAGE-PLAN-PROPOSAL
    - SB-INBOX-INTERPRET-NOTE
    - SB-CHAT-PRIVATE-ACTION-STAYS-PRIVATE
    - JRN-CHAT-REVIEW-HOUSEHOLD-ACTION
  modify:
    - FR-INBOX-CAPTURE
    - FR-INBOX-INTERPRET
    - FR-LISTS-ARCHIVE-LIST
    - FR-MEALS-SLOT-ASSIGNMENT
    - FR-RECIPES-DELETE
    - FR-WEB-CONFIRM-DISCONNECT
    - QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    - FR-CHAT-ASK-ASSISTANT
    - SB-FAMILY-MANAGER-PROVISIONS-ACCESS
    - SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
    - BR-CHAT-ASSISTANT-READ-ONLY
    - FR-AREAS-ARCHIVE-AREA
    - FR-AREAS-RENAME-AREA
    - FR-ASSISTANT-CONFIGURE-PROVIDERS
    - FR-CALENDAR-BACKGROUND-REFRESH
    - FR-CALENDAR-CANCEL-PLAN
    - FR-CALENDAR-CONFIGURE-EXTERNAL-CALENDAR
    - FR-CALENDAR-DISCONNECT-EXTERNAL-CALENDAR
    - FR-CALENDAR-RESCHEDULE-PLAN
    - FR-CALENDAR-SCHEDULE-PLAN
    - FR-LISTS-DELETE-LIST
    - FR-MEALS-APPLY-TEMPLATE
    - FR-RECIPES-UPDATE
    - FR-TASKS-DELETE-ROUTINE
    - FR-TASKS-UNASSIGN-TASK
    - UC-TASKS-UNASSIGN-TASK
    - UC-AREAS-ARCHIVE-AREA
    - CON-PRODUCT-HOUSEHOLD-FIRST
    - CON-PRODUCT-NO-MAGIC-AUTOMATION
    - CON-PRODUCT-CURRENT-SCOPE
    - JRN-MEALS-PLAN-THE-WEEK
    - SB-INBOX-APPROVE-PROPOSAL
    - SB-INBOX-APPLY-PROPOSAL
    - SB-INBOX-REJECT-PROPOSAL
    - SB-INBOX-UNAUTHORIZED-PROPOSAL
    - SB-INBOX-INVALID-PROPOSAL
    - SB-TASKS-DELETE-ROUTINE
    - SB-CALENDAR-START-SYNC
    - SB-CALENDAR-SYNC-SUCCESS
    - SB-CALENDAR-SYNC-PARTIAL-FAILURE
    - SB-CALENDAR-SYNC-FAILURE
    - SB-CALENDAR-AUTH-EXPIRED
  remove:
    - SB-LISTS-ARCHIVED-ADD-REJECTED
    - SB-MEALS-ASSIGN-RECIPE-TO-SLOT
    - SB-RECIPES-DELETE-USED-BY-PAST-OR-DRAFT
    - SB-WEB-DISCONNECT-CONFIRMATION
    - SB-INBOX-CAPTURE-AND-INTERPRET
unaffected:
  - scope: product
    cause: CON-PRODUCT-CURRENT-SCOPE
    relationship: applies-to
    cause-digest: sha256:86c4b7c550c3e4edf5c1f614510ece6a13274480c6333c7114e1a103f0219169
    reason: The scope is laid out by household capability. All existing inclusions, exclusions, provider limits and the distinction between accepted Assets intent and delivery are preserved across the product.
  - id: BC-CALENDAR
    digest: sha256:a365941e30ed9ca7e5a897db9bfbf057484fb0f87caa0c7a2dc4fdb1275c771f
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-FAMILY
    digest: sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-HOUSEHOLD-ASSETS
    digest: sha256:c906b13f8ea4ee4771790626dc32e494b4d4f705885fba1e4939e04359c498b0
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-LISTS
    digest: sha256:7a770f21b03b8d0f769f46552bc0ef53fd885d5ce0ab41d081d831d2aa74ae4c
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-MEAL-PLANNING
    digest: sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-RESPONSIBILITIES
    digest: sha256:031c9bd3da1ee33bb97c7f5b66f5d37827db17703653d1b00013680d61c55e58
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-TASKS
    digest: sha256:30abcef1a1eaa3f38dfcb0ada4d54bb3a4f83b4b9c50f1ae3d68b0faa4f835d3
    cause: CON-PRODUCT-HOUSEHOLD-FIRST
    relationship: applies-to
    cause-digest: sha256:c8abe3f8c03538b49288c014a327aa3da0f2cf0d44ea2c091aee99d34f8f663a
    reason: The editorial wording preserves household ownership, shared context and household access boundaries. This bounded context retains its responsibility and language.
  - id: BC-CALENDAR
    digest: sha256:a365941e30ed9ca7e5a897db9bfbf057484fb0f87caa0c7a2dc4fdb1275c771f
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-CHATS
    digest: sha256:8bee3e6eae0eef994be3523b72a983cd4c98e8b3aa1d365da90fd7de7d24283f
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-FAMILY
    digest: sha256:1b48c9a4b2eba0dfb34314f7afbe1b5801e4f8c41e0ab5a586a9e7da57f7ee4f
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-LISTS
    digest: sha256:7a770f21b03b8d0f769f46552bc0ef53fd885d5ce0ab41d081d831d2aa74ae4c
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-MEAL-PLANNING
    digest: sha256:bb3de4faf9d7ae3434d0db4180f005236ef81bc3ab84875aaa364e97ac3e66bd
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-RESPONSIBILITIES
    digest: sha256:031c9bd3da1ee33bb97c7f5b66f5d37827db17703653d1b00013680d61c55e58
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: BC-TASKS
    digest: sha256:30abcef1a1eaa3f38dfcb0ada4d54bb3a4f83b4b9c50f1ae3d68b0faa4f835d3
    cause: CON-PRODUCT-NO-MAGIC-AUTOMATION
    relationship: applies-to
    cause-digest: sha256:57f1dd3c04450536f959ee84803b429703e090c02f5eb7445450218da0cde161
    reason: The title and opening use direct language for the existing requirement of an explicit household decision. Existing assistant invocation, Inbox approval and owning-area rules remain unchanged in this context.
  - id: QR-MEALS-LOW-EFFORT-PLANNING
    digest: sha256:4791efc078efb88dccaddd5efad2ef16055fe6c61c8bd618f7c331bf3d0ec3da
    cause: JRN-MEALS-PLAN-THE-WEEK
    relationship: applies-to
    cause-digest: sha256:1ca92afdde559b5b4dc74423673bace6d67c62a826e7474e5321f033cb15b887
    reason: The journey now names the existing Lists handoff and opening of the generated list. It adds no required planning interaction or acceptance threshold; the low-confidence usability proposal remains draft.
  - id: UC-INBOX-CAPTURE-AND-REVIEW
    digest: sha256:3822e409a2d55ec21457dd7e181b9c0017d22ad1c5aeda1e212efcd5652f93d5
    cause: QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    relationship: applies-to
    cause-digest: sha256:90b04c52585e12c8c0083b41322685067f69f32783fd280db0e895f7714debb3
    reason: The security obligations are unchanged. Verification names explicit capture, authorization and private-Chat examples. The use case already requires household access and keeps the same failure conditions.
  - id: UC-CHAT-ASK-ASSISTANT
    digest: sha256:0d58faa2d48485053d947a4c8b633b6aa4ab261d633894f5e4fa133f4715f9d6
    cause: BR-CHAT-ASSISTANT-READ-ONLY
    relationship: governed-by
    cause-digest: sha256:d8dd787f0b4bb9bbaaac8e897ecd777ad057866d3e8288a1d060bb1c0dc6321f
    reason: Only the rule title changes to include the already accepted proposal-staging path. Explicit invocation, authorized context, private-Chat boundaries and Inbox approval remain unchanged.
  - id: SB-AREAS-ARCHIVE-AREA
    digest: sha256:b9239fe4023a7465e4f12bbdc71bc3a603959ad7827f9f305895bc25b0b485da
    cause: UC-AREAS-ARCHIVE-AREA
    relationship: illustrates
    cause-digest: sha256:bc38b2fe1288e0b32e16344abac9ec71d44081ea4d368fe55819f1c2af5e094f
    reason: The use case retains the same archiving outcome and Area-reference rules. The stale implementation note is corrected after inspecting ResponsibilityDomain.Archive; the existing example remains valid.
  - id: FR-CALENDAR-SHOW-CONNECTION-STATUS
    digest: sha256:ebea42ca547efc3df187cfb4e96c667c5f0421336e08dec667a693dbcb2b7403
    cause: SB-CALENDAR-AUTH-EXPIRED
    relationship: verification[].scenario-ref
    cause-digest: sha256:a22faad8c9b776b655d2a9815f86f0c48acfaa62ae32b42013b195e70a3c3b62
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-AUTH-EXPIRED makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause: SB-CALENDAR-AUTH-EXPIRED
    relationship: verification[].scenario-ref
    cause-digest: sha256:a22faad8c9b776b655d2a9815f86f0c48acfaa62ae32b42013b195e70a3c3b62
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-AUTH-EXPIRED makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-MANAGE-SUBSCRIPTIONS
    digest: sha256:d6628e5804a8e959841593a3a5b86044b264a42b170ee6b1233c7e6e9ebd6f61
    cause: SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
    relationship: verification[].scenario-ref
    cause-digest: sha256:089361aa142145f7056095e6f423fadcb93d2f6f1406a56707e61c650031971c
    reason: A duplicated outcome clause is removed. Revocation still denies later URL requests and changes no Plan; both verification consumers remain satisfied.
  - id: QR-CALENDAR-SUBSCRIPTION-SECURITY
    digest: sha256:e7404164d86baa68d2cc524cdb77da384d83aa22a4449f5cf94b4bf8caa02147
    cause: SB-CALENDAR-REVOKE-SECURE-SUBSCRIPTION
    relationship: verification[].scenario-ref
    cause-digest: sha256:089361aa142145f7056095e6f423fadcb93d2f6f1406a56707e61c650031971c
    reason: A duplicated outcome clause is removed. Revocation still denies later URL requests and changes no Plan; both verification consumers remain satisfied.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause: SB-CALENDAR-START-SYNC
    relationship: verification[].scenario-ref
    cause-digest: sha256:996e89b990dfbdefd982d6780bbf9852fe2974ee74b4182695f7be51bc974cb3
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-START-SYNC makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SHOW-CONNECTION-STATUS
    digest: sha256:ebea42ca547efc3df187cfb4e96c667c5f0421336e08dec667a693dbcb2b7403
    cause: SB-CALENDAR-SYNC-FAILURE
    relationship: verification[].scenario-ref
    cause-digest: sha256:419948259d4b694c298500f74d7182ac0626ed6fd3f87af7f3059b1e36141e95
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-FAILURE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause: SB-CALENDAR-SYNC-FAILURE
    relationship: verification[].scenario-ref
    cause-digest: sha256:419948259d4b694c298500f74d7182ac0626ed6fd3f87af7f3059b1e36141e95
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-FAILURE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SHOW-CONNECTION-STATUS
    digest: sha256:ebea42ca547efc3df187cfb4e96c667c5f0421336e08dec667a693dbcb2b7403
    cause: SB-CALENDAR-SYNC-PARTIAL-FAILURE
    relationship: verification[].scenario-ref
    cause-digest: sha256:b881b21e26358be7d645b83ee3908403d9a02d0ff581ee2a073eb07e16383d10
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-PARTIAL-FAILURE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause: SB-CALENDAR-SYNC-PARTIAL-FAILURE
    relationship: verification[].scenario-ref
    cause-digest: sha256:b881b21e26358be7d645b83ee3908403d9a02d0ff581ee2a073eb07e16383d10
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-PARTIAL-FAILURE makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SHOW-CONNECTION-STATUS
    digest: sha256:ebea42ca547efc3df187cfb4e96c667c5f0421336e08dec667a693dbcb2b7403
    cause: SB-CALENDAR-SYNC-SUCCESS
    relationship: verification[].scenario-ref
    cause-digest: sha256:cbbd36431def80c650c335aa667b2744301dbd0bf3d4ab7fc337faffd251f50e
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-SUCCESS makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-CALENDAR-SYNC-EXTERNAL-CALENDAR
    digest: sha256:786617e80baa09464aa3e3c5588a596444e4963880d8be17a3e48288d6fd6925
    cause: SB-CALENDAR-SYNC-SUCCESS
    relationship: verification[].scenario-ref
    cause-digest: sha256:cbbd36431def80c650c335aa667b2744301dbd0bf3d4ab7fc337faffd251f50e
    reason: The linked artifact retains its existing product obligations. SB-CALENDAR-SYNC-SUCCESS makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-FAMILY-MANAGE-MEMBER-ACCESS
    digest: sha256:47e0ea395954818d06db2c72cd7fee5b8f50cc24080212297796936e7f97edce
    cause: SB-FAMILY-MANAGER-PROVISIONS-ACCESS
    relationship: verification[].scenario-ref
    cause-digest: sha256:099af453e80ea446cad19a2659a9e7e1803f777ae127225e832667c532073129
    reason: The wording clarifies that Invited and Provisioned are labels for the same pending invitation defined in TERM-MEMBER-ACCESS. The invitation and verified-identity requirements remain unchanged.
  - id: FR-INBOX-REVIEW
    digest: sha256:43daf3b67ffad7bd4dd93b39939359941b955f0e449d04f4dab5cd5ca04e3422
    cause: SB-INBOX-APPLY-PROPOSAL
    relationship: verification[].scenario-ref
    cause-digest: sha256:3e4088750fb7da5d4ca71595e53ad7e5aee7d83f8a1ba89bf35ccd49051a762c
    reason: The linked artifact retains its existing product obligations. SB-INBOX-APPLY-PROPOSAL makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-INBOX-REVIEW
    digest: sha256:43daf3b67ffad7bd4dd93b39939359941b955f0e449d04f4dab5cd5ca04e3422
    cause: SB-INBOX-APPROVE-PROPOSAL
    relationship: verification[].scenario-ref
    cause-digest: sha256:089d5bad9e4c0cdbd7a78d761442bfaa43abf63ab70f1570e0338665ab9fc99f
    reason: The linked artifact retains its existing product obligations. SB-INBOX-APPROVE-PROPOSAL makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-INBOX-REVIEW
    digest: sha256:43daf3b67ffad7bd4dd93b39939359941b955f0e449d04f4dab5cd5ca04e3422
    cause: SB-INBOX-INVALID-PROPOSAL
    relationship: verification[].scenario-ref
    cause-digest: sha256:d30fc8b55afa4e3ec925fe21602e484ba939f7a31059254e70a9dca9c6ac7bbc
    reason: The linked artifact retains its existing product obligations. SB-INBOX-INVALID-PROPOSAL makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-INBOX-REVIEW
    digest: sha256:43daf3b67ffad7bd4dd93b39939359941b955f0e449d04f4dab5cd5ca04e3422
    cause: SB-INBOX-REJECT-PROPOSAL
    relationship: verification[].scenario-ref
    cause-digest: sha256:476c41edabc786729668b9c7f05a72c350b3465c3d598b079413fd9940a46100
    reason: The linked artifact retains its existing product obligations. SB-INBOX-REJECT-PROPOSAL makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
  - id: FR-INBOX-REVIEW
    digest: sha256:43daf3b67ffad7bd4dd93b39939359941b955f0e449d04f4dab5cd5ca04e3422
    cause: SB-INBOX-UNAUTHORIZED-PROPOSAL
    relationship: verification[].scenario-ref
    cause-digest: sha256:c799f331deda3233b0c67382da1aa6d29ebdf2b6abf084ae30c0255f4378b8f6
    reason: The linked artifact retains its existing product obligations. SB-INBOX-UNAUTHORIZED-PROPOSAL makes the same accepted behavior explicit through lifecycle coverage or editorial clarification; the verification[].scenario-ref relationship remains valid.
---

## Problem

Several examples combine different stimuli or states, and fourteen title matches occur between requirements and use cases. Some recovery notes no longer match implemented operations. Cross-area journeys do not make every ownership handoff visible.

## Intended Product Outcome

Each replacement example describes a concrete context and one stimulus. Requirements state the required outcome in their titles; use cases retain the person's goal and flow. Journeys show Chats to Inbox to Calendar and Meal Planning to Lists. Pending questions retain their draft status and evidence confidence.

## Rationale

Juan approved the improvement plan and requested Humanizer, Taste and Plain English. The editorial pass uses concrete subjects, reader-oriented headings and direct explanations. It preserves normative permissions, obligations, scope and uncertainty. No artifact is removed merely to lower the count.

## Affected Product Areas

Household coordination, Lists, Tasks, Calendar, Meal Planning, Recipes, Chats and Inbox.

## Open Questions

The four low-confidence recovered artifacts remain draft: CON-PRODUCT-PHASED-ROADMAP, QR-AREAS-GAPS-AT-A-GLANCE, QR-MEALS-LOW-EFFORT-PLANNING and UC-MEALS-UPDATE-TEMPLATE. Their proposed ordering, measurement thresholds and edit scope require owner answers. The existing deferred lifecycle questions remain explicit.

## Product Acceptance

The fourteen shared titles are clarified while preserving the existing many-to-many FR–UC relationships. Every retired example has replacement references. The complete original set of 181 structured behaviours is accounted for in the review. Household access and owning-area validation still govern cross-area actions. No confidence level is raised to hide a warning.

## Out of Scope

New household capabilities, application changes, invented acceptance thresholds and automatic resolution of product questions.
