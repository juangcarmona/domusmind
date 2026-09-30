---
id: TERM-RESPONSIBILITY-TRANSFER
type: domain-term
title: Responsibility Transfer
status: draft
defined-in: BC-RESPONSIBILITIES
synonyms:
- Transfer Responsibility
- Ownership Transfer
uses-terms:
- TERM-AREA
- TERM-AREA-OWNER
provenance:
  source: "openspec/specs/areas/spec.md (Responsibility Transfer, Notes); docs/_legacy/04_contexts/responsibilities.md; src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs; interview: product owner decision Q-0012 (E-0158)"
  confidence: high
  recovered-from: documentation
---

## Definition

The change of an Area's existing Owner to another person of the household: an explicit, traceable handover (decided: Q-0012). The Area stays active and owned throughout, and exactly one Owner exists afterwards (observed: openspec/specs/areas/spec.md, Responsibility Transfer; docs/_legacy/04_contexts/responsibilities.md, Ubiquitous Language Notes).

## Distinguish From

- Assigning an Owner: giving an Owner to an unowned Area is plain assignment. Both happen through one household action; when the Area already has an Owner, the change is a transfer (decided: Q-0012). The domain code records the previous Owner only on a transfer (observed: src/backend/DomusMind.Domain/Responsibilities/ResponsibilityDomain.cs).

## Usage

Used when accountability for an Area moves between people, for example when one parent hands school admin to the other (inferred example).
