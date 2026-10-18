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

Record a household thought or assistant action request and, when useful, turn it into an explicitly approved action in an existing product area.

## Trigger

The member has unstructured household information they want to capture or act on, or chooses to hand a supported action request from a household Chat to Inbox.

## Preconditions

- The member is authenticated and belongs to the household.
- For an assistant-generated item, the request came from a household thread and the member may access the context used.

## Main Flow

1. The member captures a text note or explicitly hands a supported household-thread action request to Inbox.
2. DomusMind stores the item as an Inbox item.
3. DomusMind may interpret it and show one or more Plan create/update, Task create, Reminder, Timeline Note, and List Item add/update proposals, including unresolved details.
4. The member reviews and may refine each proposal.
5. The member explicitly approves or rejects each proposal.
6. An approved proposal is handled through its owning product area and the member sees the result.

## Alternative Flows

- The member may leave the item pending and return to it later.
- The member may reject all proposals; no household record is created.
- A private Chat action request is not copied; the member captures the information in Inbox themselves if they choose.

## Failure Conditions

- A person without household access cannot read or act on its Inbox items.
- An incomplete or invalid proposal is not applied; the owning area reports the correction needed.
- A List Item proposal for a private or archived list the member cannot edit is not applied.

## Postconditions

- The Inbox item and its proposal decisions remain available for household review.
- Only explicitly approved proposals that pass their owning area's rules can change household state.
