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

- An **Inbox item** is a household note captured for later interpretation and review (proposed by CHG-INBOX-ASSISTED-CAPTURE).
- A **Proposal** is one suggested action derived from an Inbox item. It remains separate from the Plan or Task it may create.
- A member **approves** or **rejects** a proposal. Refining changes the proposal before approval.

## Boundaries

- Inbox owns captured text and proposal state. It does not own Plans or Tasks.
- Calendar and Tasks remain responsible for their normal commands and rules.
- Interpretation proposes structured data; it does not decide whether that data becomes household state.
- Inbox is not conversational history, external message ingestion or an autonomous agent.

## External Relationships

- Family supplies household membership and authorization.
- Calendar may receive an explicitly approved Plan proposal.
- Tasks may receive an explicitly approved Task proposal.
- Lists may receive a List item proposal only if that capability is included in the accepted V1 scope.
