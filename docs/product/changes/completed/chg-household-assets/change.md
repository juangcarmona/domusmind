---
id: CHG-HOUSEHOLD-ASSETS-001
type: product-change
title: Manage household assets, obligations and history
status: applied
base-revision: 'a0469674b98263af34a578d57a951aba1ef90a0c'
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
    - TERM-HOUSEHOLD-SETTINGS
    - UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS
    - FR-FAMILY-HOUSEHOLD-SETTINGS
  remove: []
---

## Problem

Households keep cars, homes, boilers, appliances, electronics, boats and other things that create ongoing responsibilities. The useful information is scattered across memory, paper, email and unrelated tools: what the household manages, where it is, what must happen next, who is responsible, where its paperwork is, what has already been done and what it cost. Missing a renewal, inspection, warranty deadline or service can create avoidable expense and stress. A plain inventory does not solve the continuing mental load.

Before this change, DomusMind coordinated household responsibilities, time, work and shared memory, but its product model did not provide a first-class home for managed things and their persistent obligations and history. The active Roadmap Product Change also contained low-confidence Property, Documents and Inventory proposals, including a property-maintenance use case. Household Assets is the chosen authority for obligations of a managed property. The proposal PR reconciled the independent Roadmap change by retiring its duplicate Property/Maintenance artifacts while leaving Documents and stock Inventory distinct.

## Intended Product Outcome

Household members can register and organize things their household owns or manages, including owned, rented, leased, borrowed or otherwise maintained items; associate them with a household, optional parent/containing asset, location, and optional, distinct responsible member and Area; retain relevant document metadata and authorized references; define persistent obligations; record completed and unscheduled maintenance, inspection, repair and installation history; and see upcoming due information in the unified Agenda. The product makes next actions, responsibility and evidence discoverable without turning DomusMind into an enterprise asset system, accounting tool or generic document manager.

The internal domain name is **Household Assets**, with **Asset** as a core entity. The product owner chose **Mis cosas** as the user-facing navigation label and accepted it as provisional pending household validation. “Assets” remains internal terminology; “Belongings” implies ownership and excludes managed/rented items, while “Property” suggests real estate.

The accepted product model distinguishes an `Asset` from a reusable, household-scoped `Asset Location`. A property may be an Asset with its own obligations and history, linked to a Location that other Assets can also reference. Asset containment and shared physical location are separate relationships. It also defines persistent `Asset Obligation` concepts for requirements that recur or become due; bounded, durable `Obligation Occurrence` history only when due work is recorded, completed, skipped or explicitly rescheduled; and append-oriented `Asset Records` for maintenance, repair, inspection, installation and other historical facts, including events not initiated by an obligation. These product boundaries do not choose an implementation design. Documents have metadata and authorization-bound storage references; binary storage is not assumed to exist. A provider can initially be descriptive text on a record or obligation rather than a generic Contacts context. Actual costs may be recorded against a historical record; this is not accounting.

The coherent MVP is explicit household capture and organization using a short category list with “Other”; common asset details, optional management basis and optional, distinct Area/member responsibility; one-time and calendar- or completion-relative obligations on dates in the manager-configured household timezone; a single next-due/overdue pending cycle; explicit completion, skip or reschedule with one linked history entry; ad-hoc records and actual cost with currency per record; document metadata and authorized links visible only to explicitly granted members, without binary upload; search; and read-only Agenda projection. Explicitly linked Tasks/Plans remain distinguishable Agenda items. Archived or sold Assets retain read-only history while associated document references can be removed separately. The ordinary remove action archives an Asset with history; definitive deletion is a separate explicit flow. The initial slice must prove car insurance/inspection/service, boiler warranty/service, a property with contained equipment, member/Area responsibility, and history/cost discovery. Usage-meter recurrence, legal/maintenance templates, AI extraction, automatic suggestions, contracts, financial planning and broad document management are later or deferred.

## Rationale

The product owner explicitly accepted this Product Change on 2026-11-11. The remaining details listed under Open Questions are implementation refinement or separate future scope; they do not expand the accepted first-release boundaries.

DomusMind's product promise is shared household clarity, visible responsibility and reduced mental load. Asset context is useful because it connects an enduring household thing with the information and future responsibility surrounding it. It must preserve the product's established separation of unified time (Agenda), native Calendar plans, executable Tasks and Routines, reusable Lists, responsibility Areas, and Household membership/authorization.

The initiative and ordered delivery backlog are tracked by [Epic #235: Household assets, obligations, paperwork and history](https://github.com/juangcarmona/domusmind/issues/235). This Product Change defines intent; the Epic owns implementation decomposition.

Property/location decision: the product owner answered verbatim, “Dos conceptos vinculados”. In the question's options, this meant a property Asset and a separate Location linked together. The proposed artifacts therefore let a property keep its own obligations and history while several Assets can refer to the same physical Location; linking them does not turn location into containment or legal ownership. A Location requires a name and may have an address; later lifecycle actions remain to be refined without adding a generic property-management context.

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

Accordingly, the proposed surface is named Mis cosas; Household Assets owns property obligations, with the three overlapping low-confidence Property/Maintenance artifacts retired from the independent Roadmap proposal. A Location needs only a name at capture; address is optional. Skipping a due cycle records the omission and advances to the next due state. Rescheduling moves that cycle only and preserves the recurrence rule. First-release due dates are household-local dates without an exact time. A mistaken completion is corrected by adding a traceable correction while retaining the prior outcome.

For the final seven multiple-choice decisions, the product owner's verbatim reply was “todas las recomendadas”. The selected recommendations were: manager-configured household timezone; skipped completion-relative cycles advance from their skipped due date; short category list with “Other”; document metadata visible only to explicitly granted household members; actual cost recorded with its currency per record; ordinary removal archives an Asset with history while definitive deletion requires a separate explicit flow; and linked Tasks/Calendar Plans remain two distinguishable Agenda items. The proposed Household Settings artifacts carry the timezone decision while Household retains ownership. `CHG-ROADMAP` records the Property/Maintenance retirement; Documents and stock Inventory remain independent proposals.

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
- Household Settings gains a manager-controlled timezone used for Asset Obligation local dates; Household retains ownership of the setting.
- Agenda as the unified read projection for upcoming and overdue asset obligations; no automatic Task or Calendar Plan creation.
- Optional explicit links to Tasks, Calendar plans and Lists only where a user-created execution or preparation item has independent meaning. Their owning contexts retain lifecycle and validation.
- New [Mis cosas surface specification](../../../design/surfaces/assets.md), subordinate to the accepted product model and existing surface system.
- Private document metadata and authorized links/references for the first release. No binary upload or store is established by this Product Change; a later upload slice requires a separate secure-storage decision.
- Expected OpenSpec impact after acceptance: add a Household Assets domain specification; amend Agenda projection/read navigation contract; amend Areas/Household integration contracts only if needed for stable scoped references; do not change Calendar, Tasks, Lists, Meal Planning or Areas ownership semantics.
- Expected architecture review after product acceptance and implementation: modular-monolith context map, command/query and event-projection boundaries, authorization, private document storage, recurrence and observability. Do not edit canonical architecture docs or choose infrastructure in this draft.

## Open Questions

- Which Location lifecycle actions beyond named capture and optional address are needed? The property Asset and Location are distinct linked concepts; validate their capture flow with households.
- Which specific labels belong in the initial short category list with “Other”, and which category-specific details are worth adding after common fields? Reject schema-free extensibility until evidence justifies it.
- What happens to already scheduled obligations if a manager changes the household timezone, and which calendar anchors beyond the agreed monthly month-end case are needed? Usage/meter recurrence is deferred.
- Which member actions can reverse or annotate a mistaken skip/reschedule, and how are traceable corrections shown in one history entry?
- Which document type labels and metadata beyond an authorized link are useful? Document metadata is limited to explicitly granted household members and source-content access remains independent.
- Are actual costs enough for MVP beyond amount and currency per record? Expected-cost forecasting, recurring premium ledgers, budgets and depreciation remain excluded.
- Are providers simple names on records or reusable household providers? Start with names; do not create generic Contacts/Contracts contexts absent concrete journeys.
- Which optional links to Lists add value beyond the decided explicit Task/Calendar Plan links, and what cross-surface detail should be shown? Agenda keeps linked obligation and work items distinguishable without status synchronization.
- What transfer policy applies beyond the agreed read-only history after sale/archive? Definitive deletion of an Asset with history belongs to a separate explicit flow; linked external content retains its own source policy.

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

Applying this Product Change does not implement application functionality; select a storage provider, persistence schema, framework or deployment topology; define universal legal, insurance or manufacturer obligations; provide AI/MCP or automated document extraction; generate tasks/plans/lists automatically; deliver usage-meter/IoT integrations, asset checkout, enterprise multi-company controls, depreciation, accounting, budgeting, banking, contractor marketplaces, renovation management or a standalone document-management product. The separate definitive-deletion flow is not part of the initial slice. Delivery follows the separate Epic and ordered child issues.
