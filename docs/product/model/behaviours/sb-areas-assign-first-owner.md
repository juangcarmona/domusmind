---
id: SB-AREAS-ASSIGN-FIRST-OWNER
type: structured-behaviour
title: Assigning an Owner to an unowned Area
status: draft
illustrates:
- UC-AREAS-ASSIGN-OWNER
given:
- An Area exists with no Owner
- A person of the household exists
when: The household assigns that person as Owner
then:
- The person becomes the Owner of the Area
- The Area is no longer unowned
uses-terms:
- TERM-AREA-OWNER
- TERM-OWNERSHIP-GAP
provenance:
  source: 'openspec/specs/areas/spec.md (scenario: Household assigns a primary owner to an Area)'
  confidence: high
  recovered-from: documentation
---

## Intent

Shows the gap-closing effect of assigning an Owner.

## Boundaries

None.
