---
id: TERM-OWNERSHIP-GAP
type: domain-term
title: Ownership Gap
status: draft
defined-in: BC-RESPONSIBILITIES
synonyms:
- Unowned Area
- Partially Assigned Area
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
- TERM-AREA-SUPPORT
provenance:
  source: openspec/specs/areas/spec.md (Ownership Visibility); docs/_legacy/00_product/surfaces/areas.md (Main List); docs/_legacy/04_contexts/responsibilities.md (Responsibility Coverage View)
  confidence: high
  recovered-from: documentation
---

## Definition

Missing accountability on an Area. An Area is unowned when it has no Owner, and partially assigned when it has an Owner but no Support; otherwise it is fully assigned (observed: openspec/specs/areas/spec.md, Ownership Visibility). The partially assigned distinction is informational, not a hard state (observed: openspec/specs/areas/spec.md).

## Distinguish From

- Archived Area: an Area taken out of the active view; archived is a lifecycle state, not a gap (observed: openspec/specs/areas/spec.md, Area Archiving).

## Usage

Drives the default ordering of the Areas surface (unowned, partially assigned, fully assigned, archived) and the gap indicator shown in place of a missing Owner (observed: openspec/specs/areas/spec.md, Ownership Visibility; docs/_legacy/00_product/surfaces/areas.md, Main List). The legacy context names the same grouping as a responsibility coverage view (observed: docs/_legacy/04_contexts/responsibilities.md, Read Models).
