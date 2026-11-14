---
id: CHG-STATE-MODEL-COMPLETION
type: product-change
title: Complete access lifecycle and resolve state-model questions
status: draft
base-revision: 430a49a89482609d52417a32f5dc7064fe199c71
operations:
  add:
    - LC-MEMBER-ACCESS
    - SB-FAMILY-ACCEPT-ACCESS-INVITATION
    - SB-FAMILY-REVOKE-ACCESS-INVITATION
    - SB-FAMILY-REPLACE-ACCESS-INVITATION
  modify:
    - SB-FAMILY-MANAGER-PROVISIONS-ACCESS
    - SB-FAMILY-MANAGER-DISABLES-ACCESS
    - SB-FAMILY-MANAGER-ENABLES-ACCESS
    - FR-FAMILY-MANAGE-MEMBER-ACCESS
  remove: []
---

## Problem

Access states are already accepted in terms and use cases but have no native lifecycle. Meal Plan completion, Inbox failure recovery and two calendar state entry paths remain unresolved.

## Intended Product Outcome

Household-person access has one native lifecycle with the existing four states and five state-changing transitions. Invitation replacement has its own example and retains Invited status. No FR-to-UC links change. Additional state-model changes will follow the owner's answers below.

## Rationale

Juan requested help completing the state model and asked for questions with the simplest reasonable option recommended. The access lifecycle formalizes existing accepted rules. The recommendations below remain pending and are not accepted product decisions.

## Affected Product Areas

Household and member access. Pending decisions concern Meal Planning, Inbox and external calendar connections.

## Open Questions

1. Meal Plan completion: recommend an explicit Finish week action from Active to Completed, with no automatic weekly closure or reopening. Owner answer pending.
2. Inbox recovery: recommend keeping a failed, unapplied proposal pending, showing the reason, and allowing correction and explicit reapproval. Applied proposals cannot execute again; there are no automatic retries. Owner answer pending.
3. Calendar status simplification: recommend removing Needs attention and Rehydrating from the product status catalog, keeping failure details under Failed or Authorization expired and treating a full reload as part of Syncing. This would replace part of Q-0049 and requires the owner's answer. No such change is proposed yet.

## Product Acceptance

Access remains governed by the existing manager, invitation and identity rules. Every state-changing transition has a referenced example. Replacement leaves the person Invited. Pending recommendations create no new accepted behavior or runtime change.

## Out of Scope

Applying unconfirmed product decisions, runtime implementation, first-administrator policy changes and new invitation expiration rules.
