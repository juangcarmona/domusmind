# Refinement working notes — CHG-HOUSEHOLD-ASSETS-001

This is working memory for the draft Product Change, not an accepted product decision. The owner selected a rapid batch of multiple-choice questions after the first property decision. Record each answer verbatim before changing proposed product behavior; silence is not a decision.

## Evidence and scope

- The change overlay validates with ProductShape 0.23.0: 0 errors and 4 warnings in pre-existing accepted artifacts. Baseline validation has 0 errors; its additional draft warnings belong to other live changes. `prodshape drift` reports 22 warnings in existing OpenSpec documents; none names the new Household Assets proposal.
- `TERM-ASSET`, `BR-ASSET-HIERARCHY-MEANING`, and `UC-ASSETS-REGISTER-AND-ORGANIZE` originally proposed a property as a possible Asset and permitted contained equipment. The owner selected separate linked Asset and Location concepts; `TERM-ASSET-LOCATION` now expresses that distinction.
- `BC-PROPERTY`, `TERM-PROPERTY-MAINTENANCE` and `UC-ROADMAP-PLAN-PROPERTY-MAINTENANCE` in `CHG-ROADMAP` were low-confidence V2 proposals. The owner chose Household Assets as the authority for managed-property obligations; this PR retires those three duplicate drafts from the independent Roadmap change and records why in its manifest.
- `BC-INVENTORY` in `CHG-ROADMAP` concerns pantry and supply stock. This is distinct from durable managed things and their obligations. Keep it checked and excluded from the Household Assets decision queue.
- `BC-DOCUMENTS` in `CHG-ROADMAP` concerns important household documents generally. An Asset Document is limited to paperwork associated with a managed thing. The generic context remains excluded; the owner deferred binary upload beyond the first release.
- The accepted `BC-FAMILY` owns Household and Members, including Household Settings; `BC-RESPONSIBILITIES` owns Areas; `BC-CALENDAR` owns Agenda and Plans; `BC-TASKS` owns Tasks and Routines; `BC-LISTS` owns list items. `prodshape inspect` confirms these neighbours. `prodshape impact BC-FAMILY --depth 1` shows household-first governance and membership dependencies. The proposed timezone modifies the existing Household Settings term, use case and requirement without transferring ownership.

## Prioritised candidates

| Candidate | Neighbour and relationship | Impact polarity | Disposition and reason |
| --- | --- | --- | --- |
| `TERM-ASSET`, `TERM-ASSET-LOCATION`, `BR-ASSET-HIERARCHY-MEANING`, `UC-ASSETS-REGISTER-AND-ORGANIZE` | Draft `BC-PROPERTY`, `TERM-PROPERTY-MAINTENANCE` and property-maintenance use case: overlapping meaning, no graph edge across independent live changes | Affected Asset/Location meaning; Roadmap duplicate retired | Owner chose separate linked concepts and Household Assets authority for managed-property obligations. Three low-confidence Roadmap drafts are removed, with rationale in `CHG-ROADMAP`. |
| `UC-ASSETS-MANAGE-OBLIGATION`, `UC-ASSETS-COMPLETE-OBLIGATION`, `QR-ASSETS-RECURRENCE-AND-HISTORY` | Accepted `BC-CALENDAR`/`TERM-AGENDA`: read projection and date semantics | Affected read contract; owning Calendar behavior checked and unaffected | Ask for recurrence and overdue semantics with concrete examples after property meaning is settled. |
| `FR-ASSETS-REFERENCE-DOCUMENTS`, `QR-ASSETS-HOUSEHOLD-PRIVACY` | Draft `BC-DOCUMENTS`: related subject, separate generic-document scope | Affected first-release paperwork boundary; generic context checked and excluded | Owner selected metadata and authorized references for the first release; upload is a later separately approved slice. |
| `CON-PRODUCT-HOUSEHOLD-ASSETS-BOUNDARIES` | `BC-FAMILY`, `BC-RESPONSIBILITIES`, `BC-CALENDAR`, `BC-TASKS`, `BC-LISTS` through `applies-to` | Checked and unaffected ownership | Existing context owners are explicit; no new Task, Plan or list item is created automatically. |
| `TERM-HOUSEHOLD-SETTINGS`, `UC-FAMILY-CONFIGURE-HOUSEHOLD-SETTINGS`, `FR-FAMILY-HOUSEHOLD-SETTINGS` | `BC-FAMILY` through `defined-in`/`bounded-context`; `BR-FAMILY-MANAGERS-ADMINISTER` through `governed-by`/`derived-from` | Affected Household setting, unchanged owner/manager authority | Owner selected explicit manager-controlled household timezone for Asset Obligation local dates. Proposed copies amend the existing setting contract. |
| `TERM-ASSET` | Draft `BC-INVENTORY`: stock meaning rather than managed-thing identity | Checked and excluded | Pantry and supply state stay with the separate Roadmap proposal. |

## Decision queue

1. Property meaning: answered. Separate linked Asset and Location concepts. The minimal Location fields and lifecycle are parked for later refinement.
2. Missed recurring due cycles: answered; one overdue pending until explicit resolution.
3. Monthly recurrence starting on the 31st: answered; last valid day of each month.
4. Completion-relative recurrence after late completion: answered; actual completion date.
5. Completion history: answered; one linked visible entry.
6. First-release paperwork: answered; authorized metadata/references, binary upload later.
7. Responsibility: answered; distinct optional Area and member.
8. Management basis: answered; optional.
9. Archived/sold asset history and documents: answered; read-only history, separate document removal.
10. User-facing label: answered; Mis cosas, pending household validation.
11. Roadmap Property/Maintenance overlap: answered and reconciled; Household Assets owns managed-property obligations, and the three duplicate low-confidence Roadmap drafts are retired.
12. Location capture minimum: answered; name required, address optional.
13. Explicit skip: answered; record omission and calculate next due state.
14. Explicit reschedule: answered; move only current due cycle, preserve recurrence rule.
15. Due granularity: answered; household-local date, no exact time in first release.
16. Mistaken completion: answered; append traceable correction, preserve prior outcome.
17. Source of household timezone for local-date obligations: answered; explicit manager-controlled Household Settings value.
18. Next-due anchor after skipping completion-relative work: answered; skipped due date.
19. Initial category capture: answered; short list plus Other.
20. Sensitive asset-document metadata visibility: answered; explicitly granted household members.
21. Actual-cost currency: answered; amount and currency per record.
22. Definitive asset deletion when history exists: answered; ordinary archive, separate explicit definitive-deletion flow.
23. Relationship of an explicit Task/Plan to an obligation in Agenda: answered; two distinguished linked items.

After these, revisit the user-facing label, correction/retention details and category details only where the settled model leaves a real product fork. Provider names, actual cost without accounting, Agenda as a read projection, and no generated Tasks/Plans are already stated in the draft and need no repeated question now.

## Answers and parked questions

Question 1 asked whether a property is an Asset and location together, only a location, or separate linked concepts. The owner's verbatim answer: **“Dos conceptos vinculados”**. The selected option was separate Asset and Location concepts, linked without duplicating identity or implying containment. The proposed Asset term, Location term, context boundary, hierarchy rule, registration use case, functional requirement, journey and surface draft were updated accordingly. No generic property-management behavior or Location fields were inferred.

The owner answered the first eight questions of the requested multiple-choice batch verbatim, in order:

1. “Un único pendiente vencido hasta resolverlo (recomendado)”
2. “El último día de cada mes (recomendado)”
3. “Desde la fecha real de finalización (recomendado)”
4. “Una sola entrada vinculada al cumplimiento (recomendado)”
5. “Guardar metadatos y enlaces autorizados; subir archivos en una fase posterior (recomendado)”
6. “Permitir Área y persona como datos distintos y opcionales (recomendado)”
7. “Campo opcional (recomendado)”
8. “Conservar historial en solo lectura; permitir borrar documentos por separado (recomendado)”

The answer order maps to decision queue items 2–9. `BR-ASSET-RECURRENCE-DUE-STATE` carries the three due-state decisions; linked use cases and requirements now describe one overdue pending cycle, monthly last-day behavior and actual-completion anchoring. History has one visible outcome entry and survives sale/archive as read-only. First-release document associations contain metadata and authorized references only; removing one does not delete external source content. Area/member responsibility and management basis are optional, with Area and member distinct.

The owner answered the seven questions of the second multiple-choice batch verbatim, in order:

1. “Mis cosas (recomendado)”
2. “Household Assets; reconciliar Roadmap antes de aceptar (recomendado)”
3. “Solo un nombre; dirección opcional (recomendado)”
4. “Guardar la omisión y calcular el siguiente vencimiento (recomendado)”
5. “No; mueve solo ese vencimiento y conserva la regla (recomendado)”
6. “Fecha local del hogar, sin hora exacta (recomendado)”
7. “Añadir una corrección trazable y conservar el resultado anterior (recomendado)”

The answer order maps to decision queue items 10–16. The selected label is now used in the proposed surface and navigation text; Roadmap reconciliation removes the three overlapping low-confidence drafts from that independent change. Location requires a name and may have an address. The due-state and history rules now cover explicit skip, single-cycle reschedule, local-date due semantics and append-only correction of mistaken completion.

The owner's verbatim reply to the seven recommended options above was **“todas las recomendadas”**. This selects the first option of each question in queue items 17–23: explicit manager-configured household timezone; completion-relative skip anchored to the skipped due date; short category list with Other; document metadata access granted per member; amount and currency per cost record; ordinary archive with a separate definitive-deletion flow; two distinguishable linked Agenda items. The Product Change, proposed artifacts and the independent Roadmap manifest were updated to reflect these choices.

Location lifecycle beyond initial capture, exact category labels, timezone-change migration behavior, document metadata fields and grant/revoke flow, transfer and definitive-deletion details remain for implementation refinement or a separate change. They do not alter the selected first-release boundaries.

## Readiness

The change is still a draft. The chosen label needs household validation. Roadmap Property/Maintenance is reconciled by retiring three low-confidence duplicate drafts; Documents and stock Inventory remain separate. The first-release product boundaries have owner answers. Exact labels, metadata fields, grant/revoke mechanics, timezone-change behavior, Location lifecycle and definitive deletion need implementation refinement or separate product decisions before those paths are delivered. This refinement does not approve or apply the Product Change.
