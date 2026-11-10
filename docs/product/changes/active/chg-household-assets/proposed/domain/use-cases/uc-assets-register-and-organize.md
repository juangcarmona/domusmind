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
  - TERM-ASSET-LOCATION
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
3. Member may create a household Location with a required name and optional address, or select an existing Location and link it to the Asset; the member may separately identify a containing Asset, an optional owned/rented/leased/borrowed or other management basis, and an optional Area and member as distinct responsibility facts.
4. System validates household-scoped references and creates the Asset.
5. Asset is discoverable in the household's searchable Mis cosas surface.

## Alternative Flows

- The Asset may be registered without a parent, location, management basis, member, Area or category-specific detail.
- A property may be registered as an Asset with its own obligations and history, linked to a separate Location. Equipment may refer to that Location and may separately be contained by the property Asset.

## Failure Conditions

- A referenced member, Area, parent or Location is outside the household or unavailable; a new Location has no name.
- Parent selection would create a containment cycle.

## Postconditions

One household-scoped Asset exists with optional relationships that retain distinct meanings.
