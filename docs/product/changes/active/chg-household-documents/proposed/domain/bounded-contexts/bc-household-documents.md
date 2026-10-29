---
id: BC-HOUSEHOLD-DOCUMENTS
type: bounded-context
title: Household Documents
status: draft
---
## Responsibility

Owns the household's document records: metadata, managed file versions, document-specific access policy, lifecycle and typed associations to relevant household/context entities. It makes paperwork securely retrievable without taking over the business meaning of the thing documented.

## Language

Document is the logical household record and its descriptive metadata. A Document Version is an immutable stored-file revision. A Document Association is a typed reference from a Document to an entity in its owning context. A Document Access Policy identifies the members permitted to see the record and its content. User-facing “Documents” remains provisional.

## Boundaries

- Household owns identity and membership. Household Documents scopes every Document to one household and applies document-specific access policy in addition to household authorization.
- Household Documents owns bytes for files it manages, metadata, versions, association links and document lifecycle. It does not own the target entity or its business rules.
- Household Assets owns Asset identity and the meaning of an associated warranty, service report or invoice; its current draft references to document metadata/storage require explicit alignment before implementation.
- Calendar owns Plans; Tasks owns Tasks/Routines. Their entity lifecycle remains with those contexts when a Document is associated.
- Finance and School-specific contexts are not assumed to exist; no hard dependency on them is proposed for MVP.
- Expiration is document metadata, not an automatic Task, Plan, Agenda item or generic Obligation.
- No folder hierarchy, general-purpose cloud-sync context, OCR context, or document-specific subcontext is proposed for MVP.

## External Relationships

Uses Household membership to validate household scope and member grants. Associates with selected member and existing-context entities through explicit, typed contracts; the target context retains entity identity and business meaning. Associated context visibility does not grant access to the document, and document visibility does not bypass target-context authorization. Future Asset associations depend on an accepted alignment of the separate Household Assets product model. Future MCP consumers may request authorized metadata; file-content retrieval requires a separate explicit policy.
