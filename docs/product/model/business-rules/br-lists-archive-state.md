---
id: BR-LISTS-ARCHIVE-STATE
type: business-rule
title: "Archive and restore are symmetric and preserve all list data"
status: draft
applies-to:
  - UC-LISTS-ARCHIVE-LIST
  - UC-LISTS-RESTORE-LIST
uses-terms:
  - TERM-LIST
  - TERM-ARCHIVED-LIST
provenance:
  source: "openspec/specs/lists/spec.md (List Archive, List Restore); src/backend/DomusMind.Domain/Lists/SharedList.cs (Archive, Restore)"
  confidence: "high"
  recovered-from: "documentation"
---

## Rule

Only an active list can be archived and only an archived list can be restored. Neither operation modifies or removes any item (names, quantities, notes, checked states are kept) and neither affects linked Areas or Plans (observed: openspec/specs/lists/spec.md, List Archive, List Restore; enforced in src/backend/DomusMind.Domain/Lists/SharedList.cs).

## Rationale

Lists are meant to be reused; archiving must be safe to undo.

## Examples

- Archiving a holiday packing list hides it from the list switcher; restoring it next summer brings back the same items and checked states.
- Archiving an already archived list is rejected.

## Exceptions

None.
