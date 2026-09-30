---
id: BR-AREAS-ANY-MEMBER-MANAGES
type: business-rule
title: Any household member may manage Areas
status: draft
applies-to:
- BC-RESPONSIBILITIES
- UC-AREAS-CREATE-AREA
- UC-AREAS-RENAME-AREA
- UC-AREAS-ARCHIVE-AREA
- UC-AREAS-ASSIGN-OWNER
- UC-AREAS-ADD-SUPPORT
- UC-AREAS-REMOVE-SUPPORT
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
- TERM-MEMBER
- TERM-MANAGER
provenance:
  source: "openspec/specs/areas/spec.md (requirements phrased as 'the household'); interview: product owner decision Q-0014 (E-0158)"
  confidence: high
  recovered-from: interview
---

## Rule

Any Household Member MAY create, rename and archive Areas and change their ownership (Owner and Support); these actions are not reserved to the Household Manager.

## Rationale

Areas make shared accountability visible, and the household as a whole keeps it current (decided: Q-0014). The Areas spec phrases every requirement as done by "the household" without naming a role (observed: openspec/specs/areas/spec.md).

## Examples

- A household member who is not a manager adds the "Pets" Area and makes themselves its Owner (inferred example).

## Exceptions

None. Support is treated here as part of an Area's ownership, as its domain name Secondary Owner indicates (observed: openspec/specs/areas/spec.md, Purpose).
