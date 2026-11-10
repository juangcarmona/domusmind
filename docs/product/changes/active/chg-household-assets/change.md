---
id: CHG-HOUSEHOLD-ASSETS-001
type: product-change
title: Manage household assets, obligations and history
status: draft
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - BC-HOUSEHOLD-ASSETS
    - TERM-ASSET
    - TERM-ASSET-LOCATION
    - TERM-ASSET-OBLIGATION
    - TERM-ASSET-OCCURRENCE
    - TERM-ASSET-RECORD
    - TERM-ASSET-DOCUMENT
    - BR-ASSET-HIERARCHY-MEANING
    - BR-ASSET-OBLIGATION-EXECUTION-BOUNDARY
    - BR-ASSET-OCCURRENCE-HISTORY
    - BR-ASSET-RECURRENCE-DUE-STATE
    - UC-ASSETS-REGISTER-AND-ORGANIZE
    - UC-ASSETS-MANAGE-OBLIGATION
    - UC-ASSETS-COMPLETE-OBLIGATION
    - UC-ASSETS-REVIEW-HOUSEHOLD-ASSETS
    - JRN-ASSETS-KEEP-HOUSEHOLD-READY
    - FR-ASSETS-REGISTER-AND-ORGANIZE
    - FR-ASSETS-MANAGE-OBLIGATIONS
    - FR-ASSETS-RECORD-HISTORY
    - FR-ASSETS-REFERENCE-DOCUMENTS
    - FR-ASSETS-AGENDA-PROJECTION
    - QR-ASSETS-HOUSEHOLD-PRIVACY
    - QR-ASSETS-RECURRENCE-AND-HISTORY
    - QR-ASSETS-SEARCH-AND-RESPONSIVENESS
    - CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES
  modify:
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-HOUSEHOLD-FIRST
  remove: []
---

## Problem

Households keep cars, homes, boilers, appliances, electronics, boats and other things that create ongoing responsibilities. The useful information is scattered across memory, paper, email and unrelated tools: what the household manages, where it is, what must happen next, who is responsible, where its paperwork is, what has already been done and what it cost. Missing a renewal, inspection, warranty deadline or service can create avoidable expense and stress. A plain inventory does not solve the continuing mental load.

DomusMind already coordinates household responsibilities, time, work and shared memory, but its current product model does not provide a first-class home for managed things and their persistent obligations and history. The active Roadmap Product Change also contains low-confidence proposals named Property, Documents and Inventory, including a property-maintenance use case. This initiative is independent: Household Assets is the chosen authority for obligations of a managed property, and the overlapping Roadmap draft must be reconciled before either change is accepted. This change does not modify Roadmap.

## Intended Product Outcome

Household members can register and organize things their household owns or manages, including owned, rented, leased, borrowed or otherwise maintained items; associate them with a household, optional parent/containing asset, location, and optional, distinct responsible member and Area; retain relevant document metadata and authorized references; define persistent obligations; record completed and unscheduled maintenance, inspection, repair and installation history; and see upcoming due information in the unified Agenda. The product makes next actions, responsibility and evidence discoverable without turning DomusMind into an enterprise asset system, accounting tool or generic document manager.

The provisional internal domain name is **Household Assets**, with **Asset** as a candidate core entity. The product owner chose **Mis cosas** as the user-facing navigation label. “Assets” remains internal terminology; “Belongings” implies ownership and excludes managed/rented items, while “Property” suggests real estate. Validate the chosen label with ordinary household members before acceptance.

The draft domain model distinguishes an `Asset` from a reusable, household-scoped `Asset Location`. A property may be an Asset with its own obligations and history, linked to a Location that other Assets can also reference. Asset containment and shared physical location are separate relationships. The draft also proposes persistent `Asset Obligation` aggregates for requirements that recur or become due; bounded, durable `Obligation Occurrence` history only when due work is recorded, completed, skipped or explicitly rescheduled; and append-oriented `Asset Records` for maintenance, repair, inspection, installation and other historical facts, including events not initiated by an obligation. These are candidate domain boundaries, not accepted implementation design. Documents have metadata and authorization-bound storage references; binary storage is not assumed to exist. A provider can initially be descriptive text on a record or obligation rather than a generic Contacts context. Actual costs may be recorded against a historical record; this is not accounting.

The coherent MVP is explicit household capture and organization; common asset details, optional management basis and optional, distinct Area/member responsibility; one-time and calendar- or completion-relative obligations; a single next-due/overdue pending cycle; explicit completion, skip or reschedule with one linked history entry; ad-hoc records and actual cost; document metadata and authorized links without binary upload; search; and read-only Agenda projection. Archived or sold Assets retain read-only history while associated document references can be removed separately. The initial slice must prove car insurance/inspection/service, boiler warranty/service, a property with contained equipment, member/Area responsibility, and history/cost discovery. Usage-meter recurrence, legal/maintenance templates, AI extraction, automatic suggestions, contracts, financial planning and broad document management are later or deferred.

## Rationale

DomusMind's product promise is shared household clarity, visible responsibility and reduced mental load. Asset context is useful because it connects an enduring household thing with the information and future responsibility surrounding it. It must preserve the product's established separation of unified time (Agenda), native Calendar plans, executable Tasks and Routines, reusable Lists, responsibility Areas, and Household membership/authorization.

The initiative and ordered delivery backlog are tracked by [Epic #235: Household assets, obligations, paperwork and history](https://github.com/juangcarmona/domusmind/issues/235). This Product Change defines intent; the Epic owns implementation decomposition.

Property/location decision: the product owner answered verbatim, “Dos conceptos vinculados”. In the question's options, this meant a property Asset and a separate Location linked together. The proposed artifacts therefore let a property keep its own obligations and history while several Assets can refer to the same physical Location; linking them does not turn location into containment or legal ownership. The exact location fields and lifecycle remain to be refined without adding a generic property-management context.

The product owner then selected these eight options verbatim:

- “Un único pendiente vencido hasta resolverlo (recomendado)”
- “El último día de cada mes (recomendado)”
- “Desde la fecha real de finalización (recomendado)”
- “Una sola entrada vinculada al cumplimiento (recomendado)”
- “Guardar metadatos y enlaces autorizados; subir archivos en una fase posterior (recomendado)”
- “Permitir Área y persona como datos distintos y opcionales (recomendado)”
- “Campo opcional (recomendado)”
- “Conservar historial en solo lectura; permitir borrar documentos por separado (recomendado)”

Accordingly, a missed recurring cycle stays as one overdue pending item until an explicit outcome; a monthly rule anchored to the last day of a month stays on the last valid day of each month; completion-relative rules advance from the actual completion date. Completion has one visible, linked history entry rather than duplicate rows. First-release paperwork is metadata plus authorized references, with upload deferred. Area and member responsibility are independent optional facts, as is management basis. Selling or archiving an Asset preserves read-only history, while its document associations may be removed individually; removing an association does not delete content in external storage.

The product owner then selected these seven options verbatim:

- “Mis cosas (recomendado)”
- “Household Assets; reconciliar Roadmap antes de aceptar (recomendado)”
- “Solo un nombre; dirección opcional (recomendado)”
- “Guardar la omisión y calcular el siguiente vencimiento (recomendado)”
- “No; mueve solo ese vencimiento y conserva la regla (recomendado)”
- “Fecha local del hogar, sin hora exacta (recomendado)”
- “Añadir una corrección trazable y conservar el resultado anterior (recomendado)”

Accordingly, the proposed surface is named Mis cosas; Household Assets owns property obligations, with the independent low-confidence Roadmap proposal to be reconciled before acceptance. A Location needs only a name at capture; address is optional. Skipping a due cycle records the omission and advances to the next due state. Rescheduling moves that cycle only and preserves the recurrence rule. First-release due dates are household-local dates without an exact time; how the household timezone is selected remains to be defined. A mistaken completion is corrected by adding a traceable correction while retaining the prior outcome.

Product-pattern research (reviewed 2026-10-19; vendor/project feature descriptions, not independent comparative evaluations) challenges the scope:

- [HomeBox](https://github.com/sysadminsmedia/homebox) brings item identity together with nested locations, photos, receipts/manuals, serial and warranty data, and maintenance logs. Adopt item-centered context and navigable containment; avoid treating arbitrary custom fields, QR labeling or exhaustive inventory as MVP requirements.
- [Grocy](https://grocy.info/) links household equipment and manuals with chores and due-calendar features in a broad household ERP. Adopt discoverable due work; avoid expanding into a general household ERP or duplicating DomusMind's Tasks, Lists and Agenda.
- [Snipe-IT](https://snipeitapp.com/product) and its [documentation](https://snipe-it.readme.io/docs/overview) show value in lifecycle, maintenance history and assigned location/person, but its enterprise asset tags, check-in/out, licenses, company hierarchy and administration are not household requirements.
- [Home Assistant Repairs](https://www.home-assistant.io/integrations/repairs/) represents integration/device repair issues, not durable household asset stewardship; it is not a model to copy.
- [HomeZada](https://www.homezada.com/) combines home inventory and maintenance with finance and remodeling; [Centriq](https://centriqhome.com/how-centriq-works) emphasizes manuals and model-specific service information; [HomeKeep](https://homekeep.com/how-it-works) combines home items, maintenance, paperwork and service providers. Patterns worth testing are easy capture, linked manuals/documents, reminders and history. Avoid marketplace, finance, remodel, vendor catalog or universal maintenance recommendations without validated household value.

These sources are first-party descriptions and establish feature patterns only. Validate actual terminology, capture friction and prioritization with DomusMind households; do not infer that a legal schedule or manufacturer recommendation applies universally.

## Affected Product Areas

- New internal bounded context for household asset identity, lifecycle, obligations and associated history.
- Household-scoped authorization and membership; references to Areas/member responsibility without re-owning those structures.
- Agenda as the unified read projection for upcoming and overdue asset obligations; no automatic Task or Calendar Plan creation.
- Optional explicit links to Tasks, Calendar plans and Lists only where a user-created execution or preparation item has independent meaning. Their owning contexts retain lifecycle and validation.
- New Mis cosas surface specification, subordinate to the accepted product model and existing surface system.
- Private document metadata and authorized links/references for the first release. No binary upload or store is established by this Product Change; a later upload slice requires a separate secure-storage decision.
- Expected OpenSpec impact after acceptance: add a Household Assets domain specification; amend Agenda projection/read navigation contract; amend Areas/Household integration contracts only if needed for stable scoped references; do not change Calendar, Tasks, Lists, Meal Planning or Areas ownership semantics.
- Expected architecture review after product acceptance and implementation: modular-monolith context map, command/query and event-projection boundaries, authorization, private document storage, recurrence and observability. Do not edit canonical architecture docs or choose infrastructure in this draft.

## Open Questions

- Which Location lifecycle actions beyond named capture and optional address are needed? The property Asset and Location are distinct linked concepts; validate their capture flow with households.
- Which common categories and category-specific fields are necessary for the first useful release? Prefer a small validated common model plus focused category details; reject schema-free extensibility until evidence justifies it.
- How is the household timezone chosen or changed for local-date due rules? What are calendar anchors beyond the agreed monthly month-end case and the next-due anchor after a skipped completion-relative cycle? Usage/meter recurrence is deferred.
- Which member actions can reverse or annotate a mistaken skip/reschedule, and how are traceable corrections shown in one history entry?
- Which document types/metadata and sensitive-data rules apply? Documents may include identity, vehicle or insurance details; household access is the minimum boundary, not a justification for broad exposure.
- Are actual record costs enough for MVP? Proposed scope excludes expected-cost forecasting, recurring premium ledger, multi-currency accounting, budgets and depreciation. Decide currency treatment for households with expenses in more than one currency.
- Are providers simple names on records or reusable household providers? Start with names; do not create generic Contacts/Contracts contexts absent concrete journeys.
- Should asset obligations be linked to a Task/Calendar plan explicitly, and how should Agenda present both without implying duplicate due items or synchronizing status silently?
- Household Assets is authoritative for managed-property obligations. Before acceptance, which low-confidence `CHG-ROADMAP` Property/Maintenance artifacts should be retired or reframed? The independent Roadmap change is not edited here; its Documents and stock Inventory proposals remain separate.
- What deletion/transfer and correction policy applies beyond the agreed read-only history after sale/archive and separate removal of document associations? The linked external content retains its own source policy.

## Product Acceptance

The Product Change may be accepted when product owners confirm:

- Household value is responsibility and history around managed things, not inventory completeness.
- The chosen Mis cosas user-facing label is validated with households or explicitly remains provisional.
- Asset, obligation, occurrence, record and document concepts have clear definitions and their aggregate boundaries are accepted.
- A property Asset and a reusable household Location are distinct linked concepts; parent containment, physical location, management/ownership basis and responsibility are not conflated.
- MVP recurrence modes, due/overdue, completion, skip, reschedule, history retention and idempotency semantics are explicit.
- Agenda is a projection/read surface; obligations do not become Tasks or Calendar Events, and explicit related work retains owning-context lifecycle.
- Household/member authorization, sensitive-document access and storage/retention decisions are sufficient for the chosen MVP.
- MVP and deferred scope, including no hidden automation, AI/MCP dependency, finance, generic contacts/contracts and enterprise inventory controls, are agreed.
- The overlapping Property/Maintenance draft in active `CHG-ROADMAP` is reconciled with Household Assets as the authority for managed-property obligations, without reusing or silently extending that Product Change.
- The proposed Mis cosas surface follows `docs/design/surface-system.md` and its detail/inspector interaction is usable on desktop and mobile.

## Out of Scope

This draft does not approve or apply itself; implement application functionality; select a storage provider, persistence schema, framework or deployment topology; define universal legal, insurance or manufacturer obligations; provide AI/MCP or automated document extraction; generate tasks/plans/lists automatically; deliver usage-meter/IoT integrations, asset checkout, enterprise multi-company controls, depreciation, accounting, budgeting, banking, contractor marketplaces, renovation management or a standalone document-management product. It does not modify `CHG-ROADMAP` or the canonical product model. Any resulting delivery follows the separate Epic and ordered child issues.
