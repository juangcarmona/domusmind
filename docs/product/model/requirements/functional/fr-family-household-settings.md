---
id: FR-FAMILY-HOUSEHOLD-SETTINGS
type: functional-requirement
title: "Configure household-wide settings"
status: active
derived-from:
  - UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS
  - BR-FAMILY-MANAGERS-ADMINISTER
verification:
  - scenario: "A manager sets Monday as its first day of week and the Agenda week view starts on Monday"
  - scenario: "A manager renames the household and the new name is shown to everyone"
  - scenario: "A person who is not a manager tries to change the first day of week and is rejected"
  - scenario: "A manager sets the household timezone; asset due dates use it consistently for members in other device timezones, and a non-manager cannot change it"
uses-terms:
  - TERM-HOUSEHOLD-SETTINGS
provenance:
  source: "src/backend/DomusMind.Domain/Family/Family.cs (UpdateSettings); openspec/specs/family/spec.md (NOTE N2); interview: product owner decision Q-0025 (E-0158); CHG-HOUSEHOLD-ASSETS-001 owner refinement (household timezone)"
  confidence: high
  recovered-from: observation
---

## Requirement

The product MUST let a manager set the household's name, primary language, first day of week, date format and timezone, applied to everyone in the household, and MUST reject such a change from a person who is not a manager. The first four settings are current product (decided: Q-0025; observed: src/backend/DomusMind.Domain/Family/Family.cs, UpdateSettings). The timezone is proposed by CHG-HOUSEHOLD-ASSETS-001 and governs household-local due dates independently of member device and server timezone.

## Rationale

Weeks and dates must look the same for everyone in the home; other areas depend on the first day of week.
