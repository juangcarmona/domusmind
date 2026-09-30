---
id: JRN-AREAS-CLOSE-OWNERSHIP-GAPS
type: journey
title: Close the household's ownership gaps
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
steps:
- use-case: UC-AREAS-REVIEW-OWNERSHIP
- use-case: UC-AREAS-ASSIGN-OWNER
- use-case: UC-AREAS-ADD-SUPPORT
provenance:
  source: "docs/_legacy/00_product/surfaces/areas.md (Purpose, Entry Points, Interaction, Success Criteria); openspec/specs/areas/spec.md (Ownership Visibility); interview: product owner decision Q-0015 (E-0158)"
  confidence: medium
  recovered-from: documentation
---

## Intended Outcome

Every active Area of household life has an Owner, and the household can see who supports each one (inferred from docs/_legacy/00_product/surfaces/areas.md, Purpose and Success Criteria).

## Entry Conditions

The household has Areas, some unowned; for example after Areas were added without an Owner, or during the onboarding follow-up; a new household has no default Areas for now (decided: Q-0015) (observed: docs/_legacy/00_product/surfaces/areas.md, Entry Points).

## Journey Narrative

The person opens Areas and sees unowned Areas at the top, each with a gap indicator (observed: openspec/specs/areas/spec.md, Ownership Visibility). They select an unowned Area, assign an Owner inline, and optionally add Support, without leaving the list (observed: docs/_legacy/00_product/surfaces/areas.md, Inspector and Interaction). The Area moves down to the partially or fully assigned group. They repeat until no gaps remain.

## Variants and Branches

- An Area is not needed: the person archives it instead (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).
- The Area does not exist yet: the person adds it, optionally with an Owner (observed: docs/_legacy/00_product/surfaces/areas.md, Interaction).

## Completion Conditions

No active Area appears as unowned (inferred).
