---
id: BC-HOUSEHOLD-ASSETS
type: bounded-context
title: Household Assets
status: draft
---
## Responsibility

Owns household-managed things as durable subjects of identity, lifecycle, persistent obligations, related documents/references and historical records. It also owns reusable household-scoped Locations that identify where those things are. The context helps members know what the household manages, what must happen next, where supporting information is, who is responsible and what has happened.

## Language

Internal provisional name: Household Assets. Candidate core concept: Asset. User-facing naming is unresolved; do not assume “Assets” is approved interface copy. Obligation is a continuing requirement; an occurrence is a bounded instance of its due cycle; a record is durable evidence of a historical fact.

## Boundaries

- Household owns household identity, membership and authorization boundary; this context stores scoped associations, not a second household model.
- Asset identity, lifecycle, obligation definitions and associated history belong here. A property Asset and the Location it represents are separate linked concepts; a shared Location does not create Asset containment.
- Areas owns responsibility structures; this context references an Area or member without duplicating its ownership rules or assigning work implicitly.
- Agenda remains the unified temporal read surface and may project obligation due information without owning obligations.
- Calendar owns native plans; Tasks owns tasks and routines; Lists owns list items/checklists. A link does not transfer ownership or synchronize status silently.
- First-release document metadata and authorized references may be asset-associated; binary upload and file storage are deferred pending a separate secure-storage decision.
- No separate bounded context per asset category, maintenance, providers, contracts, costs, or document type is proposed for MVP.

## External Relationships

References household scope and authorized members. May reference Areas for responsibility. Publishes/serves due information to Agenda. May link explicitly to Calendar plans, Tasks/Routines and Lists while their contexts retain command, validation and lifecycle authority. Meal Planning has no initial relationship.
