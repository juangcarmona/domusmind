---
id: TERM-HOUSEHOLD-SETTINGS
type: domain-term
title: "Household Settings"
status: draft
defined-in: BC-FAMILY
synonyms: []
uses-terms:
  - TERM-HOUSEHOLD
provenance:
  source: "src/backend/DomusMind.Domain/Family/Family.cs (UpdateSettings), src/backend/DomusMind.Domain/Family/Events/FamilySettingsUpdated.cs; openspec/specs/family/spec.md (NOTE N2); docs/_legacy/04_contexts/family.md (Commands: UpdateFamilySettings); interview: product owner decision Q-0025 (E-0158)"
  confidence: high
  recovered-from: observation
---

## Definition

Preferences that belong to the household itself and shape how the product presents time and language for everyone in it: the household name, its primary language, its first day of week and its date format (observed: src/backend/DomusMind.Domain/Family/Family.cs, UpdateSettings).

## Distinguish From

- Not a person's own preferences. The specs left household settings undefined (observed: openspec/specs/family/spec.md, NOTE N2); they are part of the baseline and only managers change them (decided: Q-0025).

## Usage

Other areas depend on the first day of week: the Agenda week view and meal plans start from it (inferred: meal-planning and agenda documents refer to "the household's configured first day of week").
