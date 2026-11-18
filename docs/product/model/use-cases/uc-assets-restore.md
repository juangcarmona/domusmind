---
id: UC-ASSETS-RESTORE
type: use-case
title: Restore an archived household-managed thing
status: active
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
  - BR-ASSET-ARCHIVE-AND-RESTORE
uses-terms:
  - TERM-ASSET
---

## Goal

Bring the same archived Asset back into active use without recreating it or losing its stored information and history.

## Trigger

A household member explicitly chooses to restore an Archived Asset.

## Preconditions

The member is authorized for the household and the Asset is Archived.

## Main Flow

1. Member chooses restoration for the Archived Asset.
2. System checks household authorization and the Asset's source state.
3. System returns the same Asset to Active, preserving its identity, stored information, relationships and history.
4. Ordinary authorized editing and use become available under their governing rules.

## Alternative Flows

- Restoration is available whether or not the Asset has recorded history.

## Failure Conditions

- The member is unauthorized or the Asset belongs to another household; the Asset remains unchanged.
- The Asset is not Archived; no restore transition occurs and the Asset remains unchanged.

## Postconditions

The same Asset is Active again. Restoration has not rewritten historical outcomes or changed independently owned concepts.
