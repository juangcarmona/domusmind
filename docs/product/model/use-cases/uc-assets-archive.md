---
id: UC-ASSETS-ARCHIVE
type: use-case
title: Archive a household-managed thing
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
  - BR-ASSET-ARCHIVE-AND-RESTORE
uses-terms:
  - TERM-ASSET
---

## Goal

Put away a household-managed thing while retaining its identity, stored information and history for consultation or restoration.

## Trigger

A household member removes an Asset through the ordinary action.

## Preconditions

The member is authorized for the household and the Asset is Active.

## Main Flow

1. Member selects the ordinary removal action for the Asset.
2. System checks household authorization and the Asset's source state.
3. System changes the same Asset to Archived and preserves its stored information, relationships and history.
4. The Asset remains available for authorized read-only consultation and explicit restoration.

## Alternative Flows

- An Asset with no history is archived in exactly the same way.

## Failure Conditions

- The member is unauthorized or the Asset belongs to another household; the Asset remains unchanged.
- The Asset is not Active; no additional archive transition occurs and the Asset remains unchanged.

## Postconditions

The same Asset is Archived. Ordinary editing is unavailable and no definitive deletion has occurred.
