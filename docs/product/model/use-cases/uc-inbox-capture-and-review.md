---
id: UC-INBOX-CAPTURE-AND-REVIEW
type: use-case
title: Capture and review a household note
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
supporting-actors: []
bounded-context: BC-INBOX
governed-by:
  - BR-INBOX-REVIEW-AND-EXECUTION
uses-terms:
  - TERM-INBOX-ITEM
  - TERM-INBOX-PROPOSAL
---

## Goal

Record a household thought and, when useful, turn it into an explicitly approved action in an existing product area.

## Trigger

The member has unstructured household information they want to capture or act on.

## Preconditions

- The member is authenticated and belongs to the household.

## Main Flow

1. The member captures a text note.
2. DomusMind stores the note as an Inbox item.
3. DomusMind may interpret it and show one or more Plan or Task proposals, including unresolved details.
4. The member reviews and may refine each proposal.
5. The member explicitly approves or rejects each proposal.
6. An approved proposal is handled through its owning product area and the member sees the result.

## Alternative Flows

- The member may leave the item pending and return to it later.
- The member may reject all proposals; no Plan or Task is created.

## Failure Conditions

- A person without household access cannot read or act on its Inbox items.
- An incomplete or invalid proposal is not applied; the owning area reports the correction needed.

## Postconditions

- The Inbox item and its proposal decisions remain available for household review.
- Only explicitly approved proposals that pass their owning area's rules can change household state.
