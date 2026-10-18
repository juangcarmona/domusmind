---
id: BC-INBOX
type: bounded-context
title: Inbox
status: draft
provenance:
  source: "GitHub issue #17; proposed scope for CHG-INBOX-ASSISTED-CAPTURE"
  confidence: high
  recovered-from: documentation
---
## Responsibility

Inbox owns captured household notes and the proposals produced from them. It gives members a place to capture unstructured information, review possible actions and choose whether any proposed action should proceed.

## Language

- An **Inbox item** is a household note captured for interpretation and review.
- A **Proposal** is one suggested action derived from an Inbox item. It remains separate from the Plan, Task or List Item it may create or update.
- A member **approves** or **rejects** a proposal. Refining changes the proposal before approval.

## Boundaries

- Inbox owns captured text and proposal state. It does not own Plans, Tasks, reminders, timeline notes or List Items.
- Calendar, Tasks and Lists remain responsible for their normal commands and rules.
- Interpretation proposes structured data; it does not decide whether that data becomes household state.
- Reminder proposals refer to an existing Plan and its Calendar-owned reminder offsets. Timeline Note proposals create a standalone dated family note owned by Calendar.
- Inbox is not conversational history, external message ingestion or an autonomous agent.

## External Relationships

- Family supplies household membership and authorization.
- Chats may hand a supported action request to Inbox as an unreviewed item; an action from a private Chat is not copied.
- Calendar may receive an explicitly approved Plan, Reminder or Timeline Note proposal.
- Tasks may receive an explicitly approved Task proposal.
- Lists may receive an explicitly approved List Item addition or update proposal, subject to list privacy and write permissions.
