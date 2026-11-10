---
id: BR-ASSET-HIERARCHY-MEANING
type: business-rule
title: Asset containment does not imply ownership or responsibility
status: draft
---
## Rule

An Asset may optionally identify another Asset as its container. Containment is optional and means only that the child is physically or conceptually part of/within the parent. A separate household Location describes where an Asset is and may be shared by several Assets, including a property Asset and its equipment. Neither containment nor sharing a Location implies legal ownership, household management basis or responsibility; those are distinct facts.

## Rationale

A property may have its own Asset record and a linked Location that a boiler also uses. The boiler may additionally be contained by the property Asset; responsibility and ownership status remain independent. Conflating these relationships would make common household situations misleading.

## Examples

- A boiler and a home Asset can share a Location, and the boiler can also be contained in the home Asset and assigned to an Area.
- A borrowed tool can be managed by the household without the household owning it.
- A detached asset has no parent.

## Exceptions

Typed relationships beyond containment are not in MVP unless concrete journeys require and define them.
