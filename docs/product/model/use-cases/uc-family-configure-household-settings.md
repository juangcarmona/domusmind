---
id: UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS
type: use-case
title: "Configure household settings"
status: active
primary-actor: ACT-HOUSEHOLD-MANAGER
supporting-actors: []
bounded-context: BC-FAMILY
governed-by:
  - BR-FAMILY-HOUSEHOLD-NAME-REQUIRED
  - BR-FAMILY-MANAGERS-ADMINISTER
uses-terms:
  - TERM-HOUSEHOLD-SETTINGS
  - TERM-HOUSEHOLD
provenance:
  source: "src/backend/DomusMind.Domain/Family/Family.cs (UpdateSettings); openspec/specs/family/spec.md (NOTE N2); docs/_legacy/04_contexts/family.md (Commands: UpdateFamilySettings, suggested RenameFamily); interview: product owner decision Q-0025 (E-0158); CHG-HOUSEHOLD-ASSETS-001 owner refinement (household timezone)"
  confidence: high
  recovered-from: observation
---

## Goal

The household's name, primary language, first day of week, date format and timezone match how the household lives. The timezone is proposed by CHG-HOUSEHOLD-ASSETS-001; other settings are current product (observed: src/backend/DomusMind.Domain/Family/Family.cs, UpdateSettings).

## Trigger

The household wants to rename itself or change how dates and weeks are shown, including the timezone used for household-local due dates.

## Preconditions

The household exists and the actor is a manager.

## Main Flow

1. The actor opens the household settings.
2. The actor changes the name, primary language, first day of week, date format or household timezone.
3. DomusMind saves the settings for the whole household.

## Alternative Flows

None known.

## Failure Conditions

- The name is empty: rejected.
- The actor is not a manager: rejected (decided: Q-0025).

## Postconditions

Every person in the household sees weeks and dates according to the new settings (inferred). Household Assets interprets local-date obligations using the configured household timezone, independent of member device and server timezone (CHG-HOUSEHOLD-ASSETS-001 owner decision).

Household settings are part of the baseline and only managers change them (decided: Q-0025). The specs had left them undefined (observed: openspec/specs/family/spec.md, NOTE N2).
