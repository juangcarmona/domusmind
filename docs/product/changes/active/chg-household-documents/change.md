---
id: CHG-HOUSEHOLD-DOCUMENTS-001
type: product-change
title: Store, find and safely share household documents
status: draft
base-revision: 'b4f77ad01ca87d2de33682d24ea2b7eee9e4cb15'
operations:
  add:
    - BC-HOUSEHOLD-DOCUMENTS
    - TERM-DOCUMENT
    - TERM-DOCUMENT-VERSION
    - TERM-DOCUMENT-ASSOCIATION
    - TERM-DOCUMENT-ACCESS-POLICY
    - BR-DOCUMENT-HOUSEHOLD-BOUNDARY
    - BR-DOCUMENT-ACCESS-INTERSECTION
    - BR-DOCUMENT-ASSOCIATION-DOES-NOT-TRANSFER-MEANING
    - BR-DOCUMENT-REPLACEMENT-PRESERVES-HISTORY
    - BR-DOCUMENT-EXPIRY-DOES-NOT-CREATE-WORK
    - UC-DOCUMENTS-UPLOAD-AND-DESCRIBE
    - UC-DOCUMENTS-ASSOCIATE-WITH-CONTEXT
    - UC-DOCUMENTS-FIND-AND-RETRIEVE
    - UC-DOCUMENTS-MANAGE-ACCESS-AND-LIFECYCLE
    - JRN-DOCUMENTS-LOCATE-AND-USE-PAPERWORK
    - FR-DOCUMENTS-UPLOAD-AND-RETRIEVE
    - FR-DOCUMENTS-METADATA-AND-ACCESS
    - FR-DOCUMENTS-CONTEXTUAL-ASSOCIATIONS
    - FR-DOCUMENTS-SEARCH
    - FR-DOCUMENTS-VERSIONS-AND-LIFECYCLE
    - QR-DOCUMENTS-ACCESS-CONTROL
    - QR-DOCUMENTS-UPLOAD-SAFETY
    - QR-DOCUMENTS-DURABILITY-AND-RECOVERY
    - QR-DOCUMENTS-SEARCH-AND-RESPONSIVE-USE
    - CON-DOCUMENTS-OWNERSHIP-BOUNDARY
  modify:
    - CON-PRODUCT-CURRENT-SCOPE
    - CON-PRODUCT-HOUSEHOLD-FIRST
  remove: []
---

## Problem

Household paperwork is scattered across paper, email, devices and unrelated storage. Members cannot reliably answer where a warranty, insurance policy, inspection report, school authorization or invoice is; whether it is current; who may see it; or what it is connected to. The current product model explicitly excludes a standalone Documents area and has no accepted document-storage capability. The repository currently has no general document/file upload, download or storage subsystem, and its self-hosted deployment documents backups for PostgreSQL but no file volume.

The independent Household Assets Product Change (#235 / Draft PR #236) proposes asset-related document metadata/references within Household Assets. That makes the boundary important: the Assets proposal currently gives the Asset context responsibility for document metadata and storage references, while this initiative proposes that Documents own file bytes, document metadata, access and lifecycle. Assets must retain the business meaning of an attached warranty or invoice. This change must not silently modify the Assets Product Change; the competing ownership proposals and required alignment are explicit dependencies for product review and delivery.

The active `CHG-ROADMAP` also proposes low-confidence `BC-DOCUMENTS`, `TERM-IMPORTANT-DOCUMENT` and a document-tracking use case. This dedicated Product Change uses a distinct context identity and proposes a complete household-document capability; the roadmap draft remains untouched and must be reconciled by the product owner before either proposal is applied.

## Intended Product Outcome

Household members can securely add, organize, find, retrieve and manage important household documents from a Documents library and from the household context where each document matters. They can record useful metadata, such as title, document type, issuer, issue/expiry dates, reference number and tags; associate a document with a household member or an authorized entity in another context; choose a deliberate audience; inspect supported previews or download; replace a file without silently destroying its prior revision; distinguish a renewed/superseding document from a correction; and archive or delete according to an explicit retention policy.

The proposed internal context is **Household Documents**. It owns document identity, metadata, stored file versions, document-specific access policy, lifecycle and typed association links. It does not own the domain meaning or lifecycle of linked entities: Household owns member identity, Assets owns the meaning and state of an asset/warranty, Calendar owns Plans, and Tasks owns Tasks. An association is a navigable relation, not a transfer of ownership, automatic task, expiration obligation or permission grant. Financial commitments and school-specific contexts are not present in the accepted model and are not hard dependencies.

For a document to be available, the requesting member must be authorized for the household, be allowed by the document's explicit audience policy, and be allowed to access any contextual target whose data is used or exposed. Association must never widen access; unknown, ambiguous or unavailable authorization is denied. Household membership alone must not imply that every document is shared. The interface must clearly distinguish household-shared from member-restricted documents and avoid leaking restricted titles, dates, categories, filenames or existence through search, linked surfaces, notifications, previews, logs or future assistant integrations.

The recommended MVP is a search-first document library; secure upload and retrieval for a deliberately small set of validated common formats; basic metadata and categories; explicit household-shared or member-restricted access; safe preview/download behavior; association with household members and selected existing context records; asset association once the Assets ownership boundary is reviewed and aligned; basic replacement history; and separate archive/delete actions governed by approved retention behavior. Expiration is visible document metadata only. It does not itself create a Task, Calendar Plan, Agenda entry or generic Obligation; the owning business context decides whether action is required.

An ordinary file replacement is proposed to create an immutable file version under the same logical Document, with the latest approved version shown by default and prior versions retained according to policy. A renewed policy or certificate may instead be a new Document that explicitly supersedes its predecessor, preserving distinct coverage periods and history. Product owners must validate this distinction. No deep folder tree or exhaustive filing taxonomy is required; search and contextual retrieval take precedence.

## Rationale

Documents are useful when members can retrieve them at the point of need, not when they must maintain a complex filing system. A dedicated library is justified because household members need search across categories and contexts, while the same document should also be reachable from its related member, asset, plan or task. A document association does not make the target context a file store.

The repository's current self-hosted runtime is a .NET API plus PostgreSQL. Compose persists PostgreSQL and Data Protection keys, with no general file volume or document-specific backup/restore procedure. Therefore the product intent requires durable storage and recoverability but leaves implementation choice open for an architecture assessment. Candidate approaches (database bytes, a protected persistent local volume, or a separately operated/object store) have different backup, restore, size, availability, encryption and operational consequences. Do not add a new managed service or claim files are backed up until the self-hosted installation and recovery path are demonstrated.

Security is a release boundary, not a later polish item. [OWASP's File Upload Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/File_Upload_Cheat_Sheet.html) calls for allow-listed file types, server-side validation, size limits, generated storage names, access controls, storage outside the web root and malware/sandbox checks where available. [Microsoft's ASP.NET Core upload guidance](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-10.0) also describes denial-of-service, malware and overwrite risks, and recommends safe names, size/type validation, isolated storage and scanning. [OWASP Authorization guidance](https://cheatsheetseries.owasp.org/cheatsheets/Authorization_Cheat_Sheet.html) recommends least privilege and deny-by-default authorization. These are security implementation guidance; exact controls and architecture require threat assessment for DomusMind's self-hosted household deployment.

The product should not turn document expiry into hidden work. An expiry date can be searched and shown on the document, but a renewal action belongs to the context or member decision that understands the underlying policy, contract, vehicle or obligation. Similarly, OCR, AI extraction and content indexing increase privacy exposure and are not required to make common paperwork findable by metadata and context.

The Assets draft needs a deliberate boundary correction before implementation: Household Documents should own Document records, storage, access, versions and lifecycle; Household Assets should own the Asset and the domain interpretation of its linked document. The Documents Epic will coordinate this as an explicit dependency while leaving PR #236 and its Product Change unchanged.

## Affected Product Areas

- New Household Documents bounded context for document identity, metadata, file versions, authorization policy, storage lifecycle and contextual associations.
- Household membership and member identity as the isolation boundary and association target; document-specific visibility must be narrower where required.
- Household Assets association for equipment/property paperwork, contingent on the separate Assets Product Change aligning its proposed document ownership.
- Calendar Plan and Tasks associations through context-owned, authorized reference contracts; no changes to those contexts' semantics or automatic expiry work.
- Dedicated Documents surface and document inspector, plus contextual retrieval from relevant Household, Assets, Calendar and Tasks surfaces.
- Self-hosted file storage, upload validation, safe retrieval, encryption, backup/recovery, retention and deletion requirements; exact infrastructure remains an architecture decision.
- Expected OpenSpec impact after product acceptance: add a Documents specification; define association contracts for Household and supported existing contexts; align the Assets proposal before treating Asset attachment as available; amend related specs only where navigation/authorization contracts change.
- Future MCP interoperability may expose authorized document metadata/search; binary content access is a separate explicit permission and product/security decision. MCP and AI are not MVP dependencies.

## Open Questions

- Which user-facing label is clearest: Documents, Paperwork, Files, or another household term? “Documents” is provisional and should be validated with household members.
- What is the correct default audience at upload: private to the uploader, selected members, or an explicit choice with no default? Recommended safety posture: require an explicit audience choice; do not infer sharing from household membership.
- Who can grant/revoke access, and can a household manager access a restricted document without an explicit grant? How do child-related documents, age, consent and household relationship rules affect access? Do not assume an adult/parent relationship grants permission; define denied-access recovery and audit expectations.
- When a member is removed, loses household access or changes role, what happens to documents they uploaded, their explicit grants, and their ability to administer them?
- Which formats are required for upload, preview and download in the MVP? Recommended initial review: PDF and common image formats, with unknown/active content download-only or rejected until safe handling is established.
- Must malware scanning be available before any file becomes retrievable, and what happens when the scanner is unavailable? Recommended release gate: quarantine and deny retrieval until validation succeeds; if no suitable scanning capability can be operated, explicitly decide whether MVP is blocked or accepts a documented residual risk.
- Which storage option fits the single-household self-hosted deployment: PostgreSQL file bytes, a dedicated persistent local volume, or external/object storage? What maximum file size, per-household quota and total capacity policy applies?
- What encryption-at-rest responsibility belongs to DomusMind versus the self-host operator? What is encrypted in transit between client, proxy, API, storage and backup?
- Which backup/restore guarantees cover metadata and file bytes together? What recovery point/time objectives, integrity checks and restore drills are acceptable for household documents?
- What retention period applies to archived, superseded and deleted documents, prior file versions and backups? Does deletion remove bytes immediately or after a defined backup-retention period? Is a recoverable trash state needed?
- Should replacing a file create a revision under one Document, while a renewed policy creates a new Document linked as superseding the old one? How are metadata changes/version corrections represented?
- Which fields are searchable? Recommended MVP excludes file contents and highly sensitive reference numbers from indexing unless an explicit access-aware design is approved.
- Which typed associations should ship first: Household Member, Calendar Plan, Task, and Asset? Are references enough, or must target contexts validate and expose safe names/navigation? Financial commitments and school contexts do not yet exist.
- The active Assets Product Change proposes `TERM-ASSET-DOCUMENT`/`FR-ASSETS-REFERENCE-DOCUMENTS` with metadata/storage references in Household Assets. How should its product owner align that model with the Documents ownership proposed here, without silently modifying PR #236?
- How should a future MCP client search metadata, and what additional consent, scopes, audit, content redaction or confirmation would be required before retrieving file contents? This is explicitly later than MVP.

## Product Acceptance

Product owners may accept this change when they confirm:

- Household Documents is a dedicated but household-focused capability, not a generic enterprise document-management system.
- Documents owns file, metadata, access, versions, associations and lifecycle; linked domains retain the business meaning and lifecycle of their own records.
- Household-wide versus restricted access, member/child privacy, grant/revocation and access intersection rules are clear and cannot leak metadata through any surface.
- MVP file types, upload limits, validation/scanning behavior, preview/download behavior, storage boundary, encryption responsibility, backup/restore and deletion/retention requirements are explicit enough to deliver safely.
- Version replacement, renewed/superseding documents, archive and delete are distinguishable and historical value is preserved as intended.
- Associations are typed and validated without hard dependencies on absent Finance or School contexts; links do not transfer ownership or grant permissions.
- Expiration remains informational unless an owning context or member explicitly creates a separate action.
- Search-first surface behavior and contextual access align with `docs/design/surface-system.md` and the proposed Documents surface spec.
- Assets Product Change #235 / Draft PR #236 has a documented alignment decision before asset association is implemented; that initiative remains otherwise untouched.
- The low-confidence document proposal in active `CHG-ROADMAP` is reconciled without reusing or silently editing that Product Change.
- OCR, AI extraction, full-text indexing, signatures, external cloud synchronization and complex workflows are clearly deferred.

## Out of Scope

This draft does not approve/apply itself, implement product behavior, choose a storage provider or database-vs-filesystem design, introduce new infrastructure, create an enterprise DMS, define a legal retention schedule, transfer business meaning from Assets/Calendar/Tasks/Household, automatically create Tasks/Plans/Agenda obligations from expiry, or grant all household members access to all documents. It does not add OCR, AI extraction/classification, full-text indexing, electronic signatures, document approval workflows, external cloud synchronization, finance, school administration or hard dependencies on absent contexts. It does not modify the Household Assets Product Change/branch/PR or active `CHG-ROADMAP`. MCP content retrieval remains a separate future decision.
