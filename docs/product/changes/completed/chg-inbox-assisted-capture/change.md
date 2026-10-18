---
id: CHG-INBOX-ASSISTED-CAPTURE
type: product-change
title: Add a review-first household Inbox
status: applied
base-revision: 'eb53e59bbe9a07029c691cc9b7c000ba19d3a047'
operations:
  add:
    - BC-INBOX
    - BR-INBOX-REVIEW-AND-EXECUTION
    - FR-INBOX-CAPTURE
    - FR-INBOX-INTERPRET
    - FR-INBOX-REVIEW
    - QR-INBOX-PRIVATE-HOUSEHOLD-DATA
    - SB-INBOX-CAPTURE-AND-INTERPRET
    - SB-INBOX-REVIEW-PROPOSAL
    - TERM-INBOX-ITEM
    - TERM-INBOX-PROPOSAL
    - UC-INBOX-CAPTURE-AND-REVIEW
  modify:
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-NO-MAGIC-AUTOMATION
  remove: []
---

## Problem

The current Product Definition makes unstructured capture and AI-assisted interpretation future capabilities. It has no household Inbox or rules for turning a note into proposed Plans and Tasks. Issue [#17](https://github.com/juangcarmona/domusmind/issues/17) also names reminders, timeline notes, voice and broad action types whose boundaries are not defined by current behavior.

## Intended Product Outcome

DomusMind has a household Inbox where a member can capture a text note, receive one or more proposed actions, review or refine them, and explicitly approve or reject each proposal. V1 supports Plan and Task proposals, uses a local interpretation runtime, and requires explicit approval before any domain change. Voice, standalone reminders, timeline notes, Lists and other action types remain out of scope for V1.

## Rationale

Issue #17 frames Inbox as a way to reduce the work of translating household thoughts into existing product concepts. The phased roadmap places AI interpretation in V3, and the no-magic-automation constraint requires household records to follow a person's decision. This approved Product Change moves review-first interpretation into the current product without implying autonomous execution or a conversational agent. Interpretation runs locally; household note text is not sent to a hosted AI provider.

## Affected Product Areas

- Household information capture and its review lifecycle.
- Calendar Plans and Tasks as the initial proposal targets.
- The current product boundary for AI-assisted interpretation.
- Privacy expectations for unstructured household text.

## Open Questions

None. Approved V1 decisions: text capture only; local interpretation; Plan and Task proposals only; each proposal requires an explicit review and decision; no standalone reminders, timeline notes, voice capture, List proposals or automatic execution.

## Product Acceptance

- The product model states who can capture and review an Inbox item and which household records may be proposed.
- Interpretation results are proposals, not commands; each proposal requires an explicit member decision before its normal area behavior can change household state.
- Rejected or unapproved proposals cause no domain change.
- The product states the privacy boundary for raw household notes and interpretation.
- Inbox remains a capture and review surface, not a chat history, messaging integration or autonomous agent.

## Out of Scope

- Voice, standalone reminder delivery, timeline notes, List proposals, external message ingestion, chat, and autonomous execution.
- Model selection, API design, persistence design, deployment topology and implementation details.
