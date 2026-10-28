---
id: FR-ASSETS-REGISTER-AND-ORGANIZE
type: functional-requirement
title: Register and organize household-managed things
status: draft
derived-from:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
verification:
  - scenario: "An authorized member registers a detached asset and an asset contained by a household property; invalid cross-household references and containment cycles are rejected."
---
## Requirement

An authorized household member can create, find and update a household-scoped managed thing with a name and broad category, and may record common details, optional containment, physical location, management basis and responsibility as distinct optional facts. References must resolve only within the same authorized household and containment must not permit cycles.

## Rationale

Low-friction capture and distinct relationships support the representative car, boiler and multi-property journeys without requiring exhaustive inventory or conflating ownership, location and responsibility.
