---
id: JRN-CHAT-REVIEW-HOUSEHOLD-ACTION
type: journey
title: Turn a household Chat request into a reviewed action
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
steps:
  - use-case: UC-CHAT-ASK-ASSISTANT
  - use-case: UC-INBOX-CAPTURE-AND-REVIEW
  - use-case: UC-CALENDAR-SCHEDULE-PLAN
provenance:
  source: 'UC-CHAT-ASK-ASSISTANT; UC-INBOX-CAPTURE-AND-REVIEW; BR-CHAT-ASSISTANT-READ-ONLY; BR-INBOX-REVIEW-AND-EXECUTION; CHG-CONVERSATIONAL-ASSISTANT; product owner, 2026-11-14: "Las tres recomendadas" (CHG-STATE-MODEL-COMPLETION)'
  confidence: high
  recovered-from: documentation
---

## Intended Outcome

A member turns a supported Plan creation request into a household commitment after reviewing its proposal.

## Entry Conditions

The member has access to a household Chat thread, Inbox and the Calendar action.

## Journey Narrative

1. The member explicitly asks @dm to prepare a Plan in a household thread.
2. The assistant clarifies required details and stages an unreviewed Inbox proposal.
3. The member opens Inbox, reviews the proposal and explicitly approves it.
4. Inbox invokes Calendar, which checks its own authorization and validation rules before creating the Plan.
5. The proposal records the successful result and links to it.

## Variants and Branches

- The member can refine, reject or leave the proposal pending.
- Supported Plan updates and List Item additions or updates use their owning area's action at step 4.
- A private Chat request is not copied. The member chooses what to enter in Inbox.
- An invalid or unauthorized action is not applied. A failed, unapplied action returns the proposal to Pending with its reason. The member may correct and explicitly approve it again; an Applied proposal cannot execute again.

## Completion Conditions

The approved action has succeeded in its owning area and the proposal is Applied. Staging or approval alone does not satisfy this condition.
