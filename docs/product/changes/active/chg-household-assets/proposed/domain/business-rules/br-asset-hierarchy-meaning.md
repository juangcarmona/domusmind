---
id: BR-ASSET-HIERARCHY-MEANING
type: business-rule
title: Asset containment does not imply ownership or responsibility
status: draft
---
## Rule

An Asset may optionally identify another Asset as its container. Containment is optional and means only that the child is physically or conceptually part of/within the parent. It does not imply legal ownership, physical address, household management basis, or responsibility; those are distinct facts.

## Rationale

A property can contain a boiler while member responsibility and ownership status remain independent. Conflating these relationships would make common household situations misleading.

## Examples

- A boiler can be contained in a home Asset and separately assigned to an Area.
- A borrowed tool can be managed by the household without the household owning it.
- A detached asset has no parent.

## Exceptions

Typed relationships beyond containment are not in MVP unless concrete journeys require and define them.
