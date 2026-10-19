---
id: UC-ASSETS-REGISTER-AND-ORGANIZE
type: use-case
title: Register and organize a household-managed thing
status: draft
primary-actor: ACT-HOUSEHOLD-MEMBER
bounded-context: BC-HOUSEHOLD-ASSETS
governed-by:
  - BR-ASSET-HIERARCHY-MEANING
uses-terms:
  - TERM-ASSET
---
## Goal

Make a thing the household manages easy to identify, locate and relate to responsibility.

## Trigger

A member wants to remember and coordinate care for a car, property, appliance, equipment or other managed thing.

## Preconditions

The member is authorized in the household. The household has not archived the target context.

## Main Flow

1. Member names the thing and selects or enters a broad category.
2. Member records only useful available details, such as make/model, serial or acquisition/installation information.
3. Member may identify a containing Asset, physical location, management basis and responsible member or Area as distinct optional details.
4. System validates household-scoped references and creates the Asset.
5. Asset is discoverable in the household's searchable Assets surface.

## Alternative Flows

- The Asset may be registered without a parent, location, member, Area or category-specific detail.
- A property may be registered and other things optionally contained within it, pending product terminology/model validation.

## Failure Conditions

- A referenced member, Area or parent is outside the household or unavailable.
- Parent selection would create a containment cycle.

## Postconditions

One household-scoped Asset exists with optional relationships that retain distinct meanings.
