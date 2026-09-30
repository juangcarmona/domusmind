---
id: BR-FAMILY-HOUSEHOLD-NAME-REQUIRED
type: business-rule
title: "A household needs a name"
status: draft
applies-to:
  - UC-FAMILY-CREATE-HOUSEHOLD
  - UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "openspec/specs/family/spec.md (Household Creation); src/backend/DomusMind.Domain/Family/ValueObjects/FamilyName.cs"
  confidence: high
  recovered-from: documentation
---

## Rule

A household must have a non-empty name; the name is the only thing needed to create one (observed: openspec/specs/family/spec.md, Household Creation). The code also limits it to 100 characters and trims surrounding spaces (observed: src/backend/DomusMind.Domain/Family/ValueObjects/FamilyName.cs).

## Rationale

The name is how people recognise their household everywhere in the product (inferred).

## Examples

- "The Garcias" creates a household.
- An empty name is rejected with a validation error (observed: openspec family spec, scenario "Household creation fails with an empty name").

## Exceptions

None.
