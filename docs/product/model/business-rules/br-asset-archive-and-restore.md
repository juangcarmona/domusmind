---
id: BR-ASSET-ARCHIVE-AND-RESTORE
type: business-rule
title: Ordinary asset removal archives and explicit restoration reactivates
status: active
uses-terms:
  - TERM-ASSET
---

## Rule

A newly registered Asset is Active. Ordinary removal of an Active Asset archives it whether or not it has history. An Archived Asset remains available to authorized household members for consultation and is read-only: ordinary editing and use cannot change it until explicit restoration returns it to Active. Restoration is the permitted state-changing action on an Archived Asset.

Archive and restore preserve the Asset's identity, household, stored information, relationships and recorded history. They do not permanently delete it or rewrite historical outcomes. Restored Assets remain subject to ordinary authorization, relationship validation and history rules.

Archive transitions only from Active to Archived; restore transitions only from Archived to Active. An action outside its source state makes no additional lifecycle transition and does not change the Asset. All actions retain the existing household/member authorization boundary.

## Rationale

Retiring a household-managed thing has one non-destructive meaning. Explicit restoration brings that same thing back into use rather than asking the household to recreate it or lose its history.

## Examples

- Removing a newly registered tool with no history archives it rather than deleting it.
- Archiving a boiler preserves its identity and recorded service history for consultation.
- Restoring that boiler returns it to Active with its stored information and history intact.
- An attempted rename while Archived is refused and leaves its name unchanged.

## Exceptions

Definitive deletion requires a separate explicit flow outside this archive/restore lifecycle. Sale-specific transitions and the lifecycles of obligations or other linked concepts are not defined by this rule. Archive and restore do not cascade state changes to those concepts or grant access to independently protected documents.
