---
id: FR-ASSETS-REGISTER-AND-ORGANIZE
type: functional-requirement
title: Register and organize household-managed things
status: active
derived-from:
  - UC-ASSETS-REGISTER-AND-ORGANIZE
verification:
  - scenario: "An authorized member registers a named Location without an address, links it to a property Asset and equipment, and may add an address later; invalid cross-household references and containment cycles are rejected."
  - scenario: "Removing an Asset with history through the ordinary action archives it; definitive deletion is unavailable through that action."
---
## Requirement

An authorized household member can create, find and update a household-scoped managed thing with a name and a broad category chosen from a short list that includes “Other”, and may record common details, optional containment, a separately created or selected household Location, optional management basis and optional Area and member responsibility as distinct facts. A new Location requires only a name; its address is optional. A property Asset and its Location remain linked but separately identifiable. Several Assets may share one Location without acquiring a containment relationship. References must resolve only within the same authorized household and containment must not permit cycles. The ordinary remove action archives an Asset with history; definitive deletion requires a separate explicit flow.

## Rationale

Low-friction capture and distinct relationships support the representative car, boiler and multi-property journeys without requiring exhaustive inventory or conflating ownership, location and responsibility.
