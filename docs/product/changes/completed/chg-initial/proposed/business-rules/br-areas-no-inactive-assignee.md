---
id: BR-AREAS-NO-INACTIVE-ASSIGNEE
type: business-rule
title: Removed or inactive people cannot receive new Area assignments
status: draft
applies-to:
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-ADD-SUPPORT
uses-terms:
- TERM-AREA
- TERM-MEMBER
provenance:
  source: "docs/_legacy/04_contexts/responsibilities.md (Invariants, Family Consistency; Domain Events Consumed); interview: product owner decision Q-0018 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Rule

A person who has been removed from the household, or is inactive, cannot be made Owner or Support of an Area.

## Rationale

Assignments must reference valid, existing people of the household (observed: docs/_legacy/04_contexts/responsibilities.md, Family Consistency). The legacy context also suggests marking Areas that need reassignment after a person is removed (observed: docs/_legacy/04_contexts/responsibilities.md, Domain Events Consumed), but no source defines that behaviour.

## Examples

(inferred) A person removed from the household no longer appears as a candidate Owner.

## Exceptions

Undecided, deferred (Q-0018): what happens to the Areas a removed person owns or supports is left to the definition of member removal (V1.1).
