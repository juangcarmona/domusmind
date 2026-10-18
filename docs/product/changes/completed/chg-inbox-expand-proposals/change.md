---
id: CHG-INBOX-EXPAND-PROPOSALS
type: product-change
title: Expand Inbox proposals and interpreter choices
status: applied
base-revision: '42a745595ce4c1cd2d40ced073a714c8be14c06e'
operations:
  add: []
  modify:
    - BC-CALENDAR
    - BC-INBOX
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-NO-MAGIC-AUTOMATION
    - CON-PRODUCT-PHASED-ROADMAP
    - FR-INBOX-INTERPRET
    - QR-INBOX-PRIVATE-HOUSEHOLD-DATA
  remove: []
---

## Problem

The approved Inbox scope covers only Plan and Task proposals and assumes local interpretation. That omits Calendar reminders and dated family timeline notes, and it ties product behavior to a particular inference deployment.

## Intended Product Outcome

Inbox accepts text notes, proposes Plans, Tasks, reminders attached to existing Plans, and standalone dated family timeline notes. Members review and approve each proposal explicitly. The deployment selects a provider-neutral text interpretation adapter by configuration, and the Inbox discloses when note text is sent outside the deployment.

## Rationale

The product owner selected text-only capture, explicit review, provider selection by configuration, and the additional Reminder and Timeline Note proposal types. Calendar already defines reminders as offsets before a Plan starts. A standalone dated note extends Calendar's timeline records while retaining household review before creation.

## Affected Product Areas

- Inbox interpretation and review.
- Calendar reminders and the household timeline.
- Household privacy disclosure for interpretation.
- Current product scope and the phased roadmap.

## Open Questions

None. A timeline note is a standalone family note with a date and text. A reminder proposal adds a positive offset to a selected existing Plan; it does not create a standalone reminder or deliver a notification.

## Product Acceptance

- The product model permits the four proposal types and keeps each proposal pending until an authorized member acts.
- Reminder proposals attach an offset to a selected existing Plan and use Calendar's existing rules.
- Timeline notes are standalone dated family records included in the household timeline.
- The configured interpreter may be local or outside the deployment, and the Inbox discloses that data flow.
- Capturing or interpreting text never changes domain state; explicit approval remains required.

## Out of Scope

- Voice capture, notification delivery, external messaging ingestion and autonomous execution.
- Provider-specific product behavior or changes to the Calendar reminder rules.
- Editing or deleting an applied timeline note.
- API, persistence, deployment and UI implementation details.
