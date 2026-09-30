---
id: BR-FAMILY-PET-RESTRICTIONS
type: business-rule
title: "Pets do not act in the product"
status: draft
applies-to:
  - UC-FAMILY-REGISTER-PET
  - UC-FAMILY-PROVISION-MEMBER-ACCESS
  - BC-FAMILY
uses-terms:
  - TERM-PET
  - TERM-MEMBER-ROLE
  - TERM-MEMBER-ACCESS
provenance:
  source: "openspec/specs/family/spec.md (Purpose, Pet Registration, Member Access Provisioning); docs/_legacy/04_contexts/family.md (Pet); docs/_legacy/04_contexts/family-member-management.md (Permission rules, Keep Pet)"
  confidence: high
  recovered-from: documentation
---

## Rule

A person with the Pet role cannot be given access, cannot be assigned tasks, cannot own areas and is left out of the Agenda's household rows and member timelines. A pet may be a participant in a plan (observed: openspec/specs/family/spec.md, Pet Registration; docs/_legacy/04_contexts/family.md, Pet).

## Rationale

Pets are part of the household but are not people who coordinate (observed: family-member-management.md, "Pets are not people").

## Examples

- The dog Rex is a participant in the plan "Vet appointment": allowed.
- Provisioning access for Rex is rejected (observed: scenario "Provisioning a Pet is rejected").
- Rex has no row in the Agenda's household view.

## Exceptions

None in V1. Pet-specific capabilities such as feeding schedules and vet tracking are planned: V2 or later (observed: family.md, Pet).
