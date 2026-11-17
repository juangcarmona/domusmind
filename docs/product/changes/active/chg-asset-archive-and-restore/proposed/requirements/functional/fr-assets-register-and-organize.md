---
id: FR-ASSETS-REGISTER-AND-ORGANIZE
type: functional-requirement
title: Register and organize household-managed things
status: active
derived-from:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
  - UC-ASSETS-ARCHIVE
  - UC-ASSETS-RESTORE
  - BR-ASSET-ARCHIVE-AND-RESTORE
verification:
  - scenario: "An authorized member registers a named Location without an address, links it to a property Asset and equipment, and may add an address later; invalid cross-household references and containment cycles are rejected."
  - scenario-ref: SB-ASSET-REGISTERED-ACTIVE
  - scenario-ref: SB-ASSET-ARCHIVED-WITHOUT-HISTORY
  - scenario-ref: SB-ASSET-ARCHIVED-WITH-HISTORY
  - scenario-ref: SB-ASSET-ARCHIVED-READ-ONLY
  - scenario-ref: SB-ASSET-ARCHIVED-CONSULTATION
  - scenario-ref: SB-ASSET-RESTORED
  - scenario-ref: SB-ASSET-UNAUTHORIZED-ARCHIVE
  - scenario-ref: SB-ASSET-UNAUTHORIZED-RESTORE
---
## Requirement

An authorized household member can create, find and update a household-scoped managed thing with a name and a broad category chosen from a short list that includes “Other”, and may record common details, optional containment, a separately created or selected household Location, optional management basis and optional Area and member responsibility as distinct facts. A new Location requires only a name; its address is optional. A property Asset and its Location remain linked but separately identifiable. Several Assets may share one Location without acquiring a containment relationship. References must resolve only within the same authorized household and containment must not permit cycles. A newly registered Asset is Active. The ordinary remove action always archives it, with or without history. An Archived Asset remains available for authorized read-only consultation. Explicit restoration returns the same Asset to Active for ordinary editing and use. Archive and restore preserve its identity, household, stored information, relationships and recorded history. Definitive deletion requires a separate explicit flow outside this archive/restore lifecycle.

## Rationale

Low-friction capture and distinct relationships support the representative car, boiler and multi-property journeys without requiring exhaustive inventory or conflating ownership, location and responsibility.
