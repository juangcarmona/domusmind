---
id: BR-AREAS-NAME-REQUIRED
type: business-rule
title: An Area must have a name of at most 100 characters
status: draft
applies-to:
- UC-AREAS-CREATE-AREA
- UC-AREAS-RENAME-AREA
uses-terms:
- TERM-AREA
provenance:
  source: openspec/specs/areas/spec.md (Area Creation); src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs
  confidence: high
  recovered-from: observation
---

## Rule

An Area must have a non-blank name of at most 100 characters; surrounding spaces are not part of the name.

## Rationale

The name is how the household recognises the Area. The name requirement is stated in the spec (observed: openspec/specs/areas/spec.md, Area Creation); the 100-character limit and trimming are enforced only by the domain code (observed: src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs).

## Examples

"  Finances " is stored as "Finances"; a blank name is rejected; a 101-character name is rejected (observed: src/backend/DomusMind.Domain/Responsibilities/ValueObjects/ResponsibilityAreaName.cs).

## Exceptions

None.
